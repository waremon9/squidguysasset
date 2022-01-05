using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : MoveCommand
{
    public override void Execute(Player player)
    {
        if (!player.IsDead) { 
            player.MoveComp.Move(Direction.Left, MoveOrigin.Normal);
        }
    }

    public override void Undo(Player player)
    {
        player.MoveComp.Move(Direction.Right, MoveOrigin.Normal);
    }
}
