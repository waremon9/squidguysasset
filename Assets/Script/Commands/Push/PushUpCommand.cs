using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushUpCommand : PushCommand
{
    public PushUpCommand(QuitPlatformEvent e) : base(e) { }
    public override void Execute(Player player)
    {
        Player p = PlayerManager.Instance.GetPlayerAtPosition(player.Coordinates + Vector2Int.up);
        if (!p) return;
        if (p.IsDead) return;
        p.MoveComp.Move(Direction.Up, MoveOrigin.Push);

        player.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        player.AnimatorPushTrigger();
    }

    public override void Undo(Player player)
    {
        throw new System.NotImplementedException();
    }
}
