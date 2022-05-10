using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;

public class DogPathfinding : MonoBehaviour
{
    public static DogPathfinding Instance;

    void Awake() => Instance = this;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public int _gridWidth;
    public int _gridHeight;
    public int _gridDetail;
    public Vector3 gridOffset;
    List<int2> _unWalkableFInt2List;

    public void SetNewUnWalkablesList()
    {
        _unWalkableFInt2List = GetUnwalkableInt2s(ObstacleManager.Instance.AllObstacles);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //var detailOffset = 1 / _gridDetail;
        for (int x = 0; x < _gridWidth * _gridDetail; x++)
        {
            for (int z = 0; z < _gridHeight * _gridDetail; z++)
            {
                var pos = new Vector3(x, 0, z) / _gridDetail + gridOffset;
                var scale = new Vector3(1, 0.1f, 1) / _gridDetail;

                Gizmos.DrawWireCube(pos, scale);
            }
        }
    }

    List<int2> GetUnwalkableInt2s(List<Transform> obstacles)
    {
        List<int2> newList = new List<int2>();

        for (int x = 0; x < _gridWidth * _gridDetail; x++)
        {
            for (int z = 0; z < _gridHeight * _gridDetail; z++)
            {
                var pos = new Vector3(x, 0, z) / _gridDetail + gridOffset;

                if (!Helper.IsPointWalkable(pos, obstacles)) newList.Add(pos.V3ToInt2());

            }
        }

    return newList;
    }


    public List<Vector3> GetVector3Path(Vector3 startingPosition, Vector3 endPosition)
    {
        NativeArray<int2> pathResult = new NativeArray<int2>(200, Allocator.Persistent);

        NativeArray<int2> unWalkableInt2s = new NativeArray<int2>(_unWalkableFInt2List.Count, Allocator.Persistent);

        for (int i = 0; i < _unWalkableFInt2List.Count; i++)
        {
            unWalkableInt2s[i] = _unWalkableFInt2List[i] - gridOffset.V3ToInt2();
        }

        FindPathJob findPathJob = new FindPathJob
        {
            startingPosition = startingPosition.V3ToInt2(),
            endPosition = endPosition.V3ToInt2(),
            unWalkables = unWalkableInt2s,
            gridWidth = _gridWidth,
            gridHeight = _gridHeight,
            gridDetail = _gridDetail,
            result = pathResult,
        };

        JobHandle jobHandle = findPathJob.Schedule();

        jobHandle.Complete();

        var finalPath = findPathJob.result.ConvertNativePathToV3Path();

        finalPath.Reverse();

        for (int i = 0; i < finalPath.Count; i++)
        {
            finalPath[i] /= _gridDetail;
            finalPath[i] += gridOffset;
            //
            
            
            //finalPath[i].ShuffleV3();
        }

        if (finalPath.Count != 0) finalPath.RemoveAt(0);
        if (finalPath.Count != 0) finalPath.RemoveAt(0);


        pathResult.Dispose();
        unWalkableInt2s.Dispose();

        return finalPath;
    }

    [BurstCompile]
    private struct FindPathJob : IJob
    {
        public int2 startingPosition;
        public int2 endPosition;
        [ReadOnly] public NativeArray<int2> unWalkables;
        public NativeArray<int2> result;
        public int gridWidth;
        public int gridHeight;
        public int gridDetail;

        public void Execute()
        {
            int2 gridSize = new int2(gridWidth, gridHeight) * gridDetail; // CHANGE TO MATCH THE INSPECTOR

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode();
                    pathNode.x = x;
                    pathNode.y = y;

                    pathNode.index = CalculateIndex(x, y, gridSize.x);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                    pathNode.CalculateFCost();

                    pathNode.isWalkable = true;
                    pathNode.cameFromNodeIndex = -1;

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            for (int i = 0; i < unWalkables.Length; i++)
            {
                var pos = unWalkables[i];
                PathNode walkablePathNode = pathNodeArray[CalculateIndex(pos.x, pos.y, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(pos.x, pos.y, gridSize.x)] = walkablePathNode;
            }

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);

            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
            neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
            neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
            neighbourOffsetArray[7] = new int2(+1, +1); // Right Up

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);


            PathNode startNode = pathNodeArray[CalculateIndex(startingPosition.x, startingPosition.y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex)
                {
                    //REACHED our destination
                    break;
                }

                // REMOVE current node from open List
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        // Neighbour not valid position
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    if (closedList.Contains(neighbourNodeIndex))
                    {
                        // ALREADY searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable)
                    {
                        // NOT WALKABLE
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.CalculateFCost();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;

                        if (!openList.Contains(neighbourNode.index))
                        {
                            openList.Add(neighbourNode.index);
                        }
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1)
            {
                // DIDN'T FIND A PATH!!!
                Debug.Log("Didn't find a path");
            }
            else
            {
                var path = CalculatePath(pathNodeArray, endNode).CopyInt2To();
                //Debug.Log("Original path length: " + path.Length);
                
                for (int i = 0; i < path.Length; i++)
                {
                    result[i] = path[i];
                }

                path.Dispose();
            }


            pathNodeArray.Dispose();
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }

        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
        {
            if (endNode.cameFromNodeIndex == -1)
            {
                // DIDN'T FIND A PATH!!!
                return new NativeList<int2>(Allocator.Temp);
            }
            else
            {
                // FOUND A PATH!
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;
                while (currentNode.cameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }

                return path;

            }
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        public int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCodePathNode = pathNodeArray[openList[0]];
            for (int i = 0; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.fCost < lowestCodePathNode.fCost)
                {
                    lowestCodePathNode = testPathNode;
                }
            }
            return lowestCodePathNode.index;
        }
    }

    


   
    public struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;

        public int cameFromNodeIndex;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool set)
        {
            isWalkable = set;
        }
    }
}
