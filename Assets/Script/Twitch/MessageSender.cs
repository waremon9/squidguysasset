using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MySingleton<MessageSender>
{
    public override bool DoDestroyOnLoad { get; }

    [SerializeField] TwitchManager twitchManager;

    public void SendMessageToTwitch(string message, string username = null)
    {
        if(username == "" || username == null)
            twitchManager.SendMessageToTwitchChat(message);
        else
        {
            twitchManager.SendMessageToTwitchChat(message,username);
        }
    }
}
