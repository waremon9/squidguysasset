using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFactory : MonoBehaviour
{
    [SerializeField] private QuitPlatformEvent quitPlatformEvent;

    #region Move Command
    public MoveUpCommand CreateMoveUpCommand()
    {
        return new MoveUpCommand();
    }
    public MoveDownCommand CreateMoveDownCommand()
    {
        return new MoveDownCommand();
    }
    public MoveLeftCommand CreateMoveLeftCommand()
    {
        return new MoveLeftCommand();
    }
    public MoveRightCommand CreateMoveRightCommand()
    {
        return new MoveRightCommand();
    }
    #endregion Move Command

    #region Push Command
    public PushUpCommand CreatePushUpCommand()
    {
        return new PushUpCommand(quitPlatformEvent);
    }
    public PushDownCommand CreatePushDownCommand()
    {
        return new PushDownCommand(quitPlatformEvent);
    }
    public PushLeftCommand CreatePushLeftCommand()
    {
        return new PushLeftCommand(quitPlatformEvent);
    }
    public PushRightCommand CreatePushRightCommand()
    {
        return new PushRightCommand(quitPlatformEvent);
    }
    #endregion Push Command

    #region Canon Command

    public InvokeCanonCommand CreateInvokeCanonCommand()
    {
        return new InvokeCanonCommand();
    }

    public ShootCanonCommand CreateShootCanonCommand()
    {
        return new  ShootCanonCommand();
    }
    #endregion
}
