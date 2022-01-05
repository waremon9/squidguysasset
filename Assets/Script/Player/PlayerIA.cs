using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIA : Player
{
    public void RandomMove()
    {
        System.Array A = System.Enum.GetValues(typeof(Direction));
        Direction V = (Direction)A.GetValue(UnityEngine.Random.Range(0, A.Length));

        MoveComp.Move(V, MoveOrigin.Normal);
    }
}