using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB2D
{
    private Vector2 _min = Vector2.zero;
    private Vector2 _max = Vector2.zero;

    public AABB2D(Vector2 min, Vector2 max)
    {
        _min = min;
        _max = max;
    }

    public static bool IsIntersect(AABB2D a, AABB2D b)
    {
        if (a._max.x < b._min.x || a._min.x > b._max.x)
        {
            return false;
        }
        if (a._max.y < b._min.y || a._min.y > b._max.y)
        {
            return false;
        }
        return true;
    }


}
