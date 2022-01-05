using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRightCommand : PushCommand
{
    public PushRightCommand(QuitPlatformEvent e) : base(e) { }
    public override void Execute(Player player)
    {
        Player p = PlayerManager.Instance.GetPlayerAtPosition(player.Coordinates + Vector2Int.right);
        if (!p) return;
        if (p.IsDead) return;
        p.MoveComp.Move(Direction.Right, MoveOrigin.Push);
        
        player.transform.rotation = Quaternion.LookRotation(Vector3.right);
        player.AnimatorPushTrigger();
    }

    public override void Undo(Player player)
    {
        throw new System.NotImplementedException();
    }
}
