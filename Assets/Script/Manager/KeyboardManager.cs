using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MySingleton<KeyboardManager>
{
    public override bool DoDestroyOnLoad { get; }

    [SerializeField] private InputEvent InputEvent;

    private void Update()
    {
        if (Input.GetKeyDown("up")) { InputEvent.Raise(PlayerManager.Instance.GetPlayerByIndex(0), CommandType.MoveUp); }
        if (Input.GetKeyDown("down")) { InputEvent.Raise(PlayerManager.Instance.GetPlayerByIndex(0), CommandType.MoveDown); }
        if (Input.GetKeyDown("left")) { InputEvent.Raise(PlayerManager.Instance.GetPlayerByIndex(0), CommandType.MoveLeft); }
        if (Input.GetKeyDown("right")) { InputEvent.Raise(PlayerManager.Instance.GetPlayerByIndex(0), CommandType.MoveRight); }
    }
}
