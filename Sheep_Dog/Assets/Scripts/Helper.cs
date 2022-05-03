using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
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
