using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExecutor : MySingleton<CommandExecutor>
{
    [SerializeField] private ListCommand ListCommand;

    public override bool DoDestroyOnLoad { get; }

    public void ExecutePushCommand()
    {
        foreach (KeyValuePair<Player, ICommand> command in ListCommand.DicoCommands)
        {
            if (command.Value is PushCommand)
            {
                command.Value.Execute(command.Key);
            }
        }
    }
    public void ExecuteMoveCommand()
    {
        foreach (KeyValuePair<Player, ICommand> command in ListCommand.DicoCommands)
        {
            if (command.Value is MoveCommand)
            {
                command.Value.Execute(command.Key);
            }
        }
        ClearList();
    }

    public void ClearList()
    {
        ListCommand.DicoCommands.Clear();
    }
}