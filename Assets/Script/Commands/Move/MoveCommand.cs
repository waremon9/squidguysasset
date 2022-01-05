using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveCommand : ICommand
{
    public abstract void Execute(Player player);
    public abstract void Undo(Player player);
}
