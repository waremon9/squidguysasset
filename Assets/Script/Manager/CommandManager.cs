using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MySingleton<CommandManager>
{
    public override bool DoDestroyOnLoad { get; }

    [SerializeField] private CommandFactory CommandFactory;
    [SerializeField] private ListCommand ListCommand;
    [SerializeField] private CommandExecutor CommandExecutor;

    private void Start()
    {
        ListCommand.Clear();
    }

    public void CreateCommand(Player p, CommandType CT)
    {
        ICommand Command;
        switch (CT)
        {
            case CommandType.MoveUp:
                Command = CommandFactory.CreateMoveUpCommand();
                break;
            case CommandType.MoveDown:
                Command = CommandFactory.CreateMoveDownCommand();
                break;
            case CommandType.MoveLeft:
                Command = CommandFactory.CreateMoveLeftCommand();
                break;
            case CommandType.MoveRight:
                Command = CommandFactory.CreateMoveRightCommand();
                break;
            case CommandType.PushUp:
                Command = CommandFactory.CreatePushUpCommand();
                break;
            case CommandType.PushDown:
                Command = CommandFactory.CreatePushDownCommand();
                break;
            case CommandType.PushLeft:
                Command = CommandFactory.CreatePushLeftCommand();
                break;
            case CommandType.PushRight:
                Command = CommandFactory.CreatePushRightCommand();
                break;
            case CommandType.InvokeCanon:
                Command = CommandFactory.CreateInvokeCanonCommand();

                break;
            case CommandType.ShootCanon:
                Command = CommandFactory.CreateShootCanonCommand();
                break;
            default:
                Debug.LogError("Command type do not exist " + gameObject.name);
                Command = null;
                break;
        }

        ListCommand.AddCommand(p, Command);

    }
}

public enum CommandType { MoveUp, MoveDown, MoveLeft, MoveRight, PushUp, PushDown, PushLeft, PushRight, Join, StartGame, ChangeVolume, InvokeCanon, ShootCanon}