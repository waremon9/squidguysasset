using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CannonData
{
    public Vector2Int coordinate;
    public Direction dir;

    public CannonData(Vector2Int coord, Direction d)
    {
        coordinate = coord;
        dir = d;
    }
}
