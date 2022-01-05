using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownCommand : MoveCommand
{

    public override void Execute(Player player)
    {
        if (!player.IsDead) { 
            player.MoveComp.Move(Direction.Down, MoveOrigin.Normal);
        }
    }

    public override void Undo(Player player)
    {
        player.MoveComp.Move(Direction.Up, MoveOrigin.Normal);
    }
}
