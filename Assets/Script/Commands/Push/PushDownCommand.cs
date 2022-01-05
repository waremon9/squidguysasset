using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDownCommand : PushCommand
{
    public PushDownCommand(QuitPlatformEvent e) : base(e) { }
    public override void Execute(Player player)
    {
        Player p = PlayerManager.Instance.GetPlayerAtPosition(player.Coordinates + Vector2Int.down);
        if (!p) return;
        if (p.IsDead) return;
        p.MoveComp.Move(Direction.Down, MoveOrigin.Push);
        
        player.transform.rotation = Quaternion.LookRotation(Vector3.back);
        player.AnimatorPushTrigger();
    }

    public override void Undo(Player player)
    {
        throw new System.NotImplementedException();
    }
}
