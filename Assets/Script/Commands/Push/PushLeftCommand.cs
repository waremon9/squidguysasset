using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushLeftCommand : PushCommand
{
    public PushLeftCommand(QuitPlatformEvent e) : base(e) { }

    public override void Execute(Player player)
    {
        Player p = PlayerManager.Instance.GetPlayerAtPosition(player.Coordinates + Vector2Int.left);
        if (!p) return;
        if (p.IsDead) return;
        p.MoveComp.Move(Direction.Left, MoveOrigin.Push);

        player.transform.rotation = Quaternion.LookRotation(Vector3.left);
        player.AnimatorPushTrigger();
    }

    public override void Undo(Player player)
    {
        throw new System.NotImplementedException();
    }
}