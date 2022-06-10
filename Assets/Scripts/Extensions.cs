using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector3 Abs(this Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    public static float GetValueByDirection(this Vector3 v, Vector3 dir)
    {
        if (dir.x != 0) return v.x;
        if (dir.y != 0) return v.y;
        if (dir.z != 0) return v.z;
        return 0;
    }
}
