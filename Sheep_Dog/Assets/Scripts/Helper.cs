using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public static class Helper 
{
    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
    public static bool IsPointWalkable(Vector3 point, List<Transform> obj)
    {
        foreach (Transform t in obj)
        {
            Bounds bounds = t.GetComponent<Collider>().bounds;
            if (bounds.Contains(point)) return false;
            
        }

        return true; // NO TRANSFORMS IN THE LIST

    }

    public static bool AreListsMatching<T>(this List<T> firstList, List<T> secondList)
    {
        if (firstList.Count != secondList.Count) return false; // IF COUNT SIN'T EQUAL, FALSE

        foreach (T item in firstList)
        {
            if (secondList.Contains(item)) // IF BOTH LISTS CONTAIN THIS ITEM
            {
                int indexA = firstList.IndexOf(item);
                int indexB = secondList.IndexOf(item);

                if (!(indexA == indexB)) return false; // IF ITEM IS IN DIFFERENT INDEX
            }
            else return false;
        }

        return true;
    }

    public static List<Vector3> ConvertNativePathToV3Path(this NativeArray<int2> oldArray)
    {
        var list = new List<Vector3>();

        bool foundOrigin = false;

        for (int i = 0; i < oldArray.Length; i++)
        {
            if (Equals(oldArray[i], int2.zero))
            {
                if (foundOrigin) continue;
                else
                {
                    list.Add(oldArray[i].Int2ToGroundedV3());
                    foundOrigin = true;
                }
            }
            else list.Add(oldArray[i].Int2ToGroundedV3());
        }

        return list;
    }

    public static Vector3 ShuffleV3(this Vector3 v3)
    {
        var newX = v3.x + UnityEngine.Random.Range(-100, 100) / 1000;
        var newY = v3.y + UnityEngine.Random.Range(-100, 100) / 1000;
        var newZ = v3.z + UnityEngine.Random.Range(-100, 100) / 1000;

        return new Vector3((float)newX, (float)newY, (float)newZ);
    }

    public static Vector3Int V3ToInt(this Vector3 v3)
    {
        var newX = (int)v3.x;
        var newY = (int)v3.y;
        var newZ = (int)v3.z;

        return new Vector3Int (newX, newY, newZ);
    }

    public static Vector3 Int2ToGroundedV3(this int2 v2)
    {
        var newX = v2.x;
        var newY = 0;
        var newZ = v2.y;

        return new Vector3(newX, newY, newZ);
    }

    public static int2 V3ToInt2(this Vector3 v3)
    {
        var newX = (int)v3.x;
        var newY = (int)v3.z;
        return new int2(newX, newY);
    }

    public static NativeArray<int2> CopyInt2To(this NativeList<int2> list)
    {
        NativeArray<int2> newArray = new NativeArray<int2>(list.Length, Allocator.Temp);

        for (int i = 0; i < list.Length; i++)
        {
            newArray[i] = list[i];
        }

        return newArray;
    }

    public static int2 V2toInt2(this Vector2 v2)
    {
        var newX = (int)v2.x;
        var newY = (int)v2.y;
        return new int2(newX, newY);
    }

    public static Quaternion SetZRotation(this Quaternion oldRot, float newZRot)
    {
        var newX = oldRot.x;
        var newY = oldRot.y;
        var newZ = newZRot;
        var newW = oldRot.w;

        var newRot = new Quaternion(newX, newY, newZ, newW);
        return newRot;
    }

    public static float DistanceF3(float3 a, float3 b)
    {
        float3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        return math.sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    public static Vector3 float3ToVector3(this float3 f3)
    {
        var x = f3.x;
        var y = f3.y;
        var z = f3.z;
        var newV3 = new Vector3(x, y, z);
        return newV3;
    }

    public static float3 Vector3ToFloat3(this Vector3 v3)
    {
        var x = v3.x;
        var y = v3.y;
        var z = v3.z;
        var newF3 = new float3(x, y, z);
        return newF3;
    }

    public static float3 NormaliseFloat3(this float3 vec3)
    {
        var sqrMag = vec3.Float3SqrMagnitude();

        if (sqrMag > 0) return vec3 / sqrMag;
        else return 0;
    }

    public static float Float3SqrMagnitude(this float3 vec3)
    {
        var aSquared = vec3.x * vec3.x;
        var bSquared = vec3.y * vec3.y;
        var cSquared = vec3.z * vec3.z;

        var fTotal = aSquared + bSquared + cSquared;

        return math.sqrt(fTotal);
    }

    public static Vector3 ConvertV2ToV3(this Vector2 v2)
    {
        var newV3 = new Vector3(v2.x, 0, v2.y);
        return newV3;
    }

    public static Vector3 GroundV3(this Vector3 oldV3)
    {
        var newV3 = new Vector3(oldV3.x, 0, oldV3.z);
        return newV3;
    }
}
