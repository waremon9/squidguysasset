using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PlatformData
{
    public int durability;
    public Vector2Int position;

    public PlatformData(Platform p)
    {
        durability = p.durability;
        position = p.Coordinates;
    }
}
