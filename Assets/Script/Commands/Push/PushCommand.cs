using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PushCommand : ICommand
{
    [SerializeField] protected QuitPlatformEvent quitPlatformEvent;

    public PushCommand(QuitPlatformEvent e)
    {
        quitPlatformEvent = e;
    }
    public abstract void Execute(Player player);
    public abstract void Undo(Player player);
}
