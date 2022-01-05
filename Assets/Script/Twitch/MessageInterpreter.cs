using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageInterpreter : MonoBehaviour
{
    [SerializeField]
    private TwitchInputs twitchInputs;

    [SerializeField]
    private PlayerJoinEvent joinEvent;

    [SerializeField]
    private InputEvent inputEvent;

    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private StartGame startGameEvent;

    [SerializeField]
    private TwitchModerator twitchModerator;

    [SerializeField]
    private ChangeVolumeEvent changeVolumeEvent;
    public void GetMessage(string message, string username, string tags)
    {
        string messageID = "";
        bool isModo = false;
        Color usernameColor = Color.white;
        List<string> tagsInList = new List<string>(tags.Split(';'));
        foreach (string tag in tagsInList)
        {
            if (tag.Contains("badges="))
            {
                if(tag.Contains("moderator") || tag.Contains("broadcaster"))
                {
                    isModo = true;
                }
            }
            else if (tag.StartsWith("id="))
            {
                messageID = tag.Remove(0,3);
            }
            else if (tag.StartsWith("color="))
            {
                string tempTag = tag.Remove(0, 6);
                ColorUtility.TryParseHtmlString(tempTag, out usernameColor);
            }
        }

        MessageToInput(message, username, messageID, usernameColor,isModo);
    }

    void MessageToInput(string message, string username, string messageID, Color usernameColor,bool isModo = false)
    {
        Player player = null;
        if (gameState.gState != GState.Join)
            player = PlayerManager.Instance.GetPlayerByName(username);

        List<string> wordsOfMessage = new List<string>(message.Split(" "[0]));
        foreach (GameInputs input in twitchInputs.gameInputs)
        {
            
            if (input.commandText.Contains(wordsOfMessage[0]) && input.CType == CommandType.Join)
            {
                if(gameState.gState == GState.Join && player == null)
                {
                    joinEvent.Raise(username, usernameColor);
                }
            }
            else if (input.commandText.Contains(wordsOfMessage[0]) && input.CType == CommandType.StartGame && isModo)
            {
                if (gameState.gState == GState.Join)
                {
                    startGameEvent.Raise(username);
                }
            }
            else if (input.commandText.Contains(wordsOfMessage[0]) && input.CType == CommandType.ChangeVolume && isModo)
            {
                float newVol;
                if(float.TryParse(wordsOfMessage[1], out newVol))
                {
                    changeVolumeEvent.Raise(newVol/100);
                }
            }
            else if (input.commandText.Contains(wordsOfMessage[0]))
            {
                inputEvent.Raise(player, input.CType);
            }
        }
    }
}
