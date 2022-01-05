using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public string username;
    public Vector2Int position;
    public Color usernameColor;

    public PlayerData(Player p)
    {
        username = p.Name;
        position = p.Coordinates;
        usernameColor = GameManager.Instance.listUsernames.GetPlayerColor(p.Name);
    }
}
