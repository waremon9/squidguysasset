using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public ListUsernames playerUsernames;

    
    private void Start()
    {
        playerUsernames.playersUsernames.Clear();
    }
    public void OnPlayerJoin(string username, Color usernameColor)
    {
        if (!playerUsernames.playersUsernames.ContainsKey(username))
        {
            playerUsernames.playersUsernames.Add(username, usernameColor);
        }
    }
}
