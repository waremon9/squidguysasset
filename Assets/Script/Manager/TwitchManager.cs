using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;

public class TwitchManager : MySingleton<TwitchManager>
{
    public override bool DoDestroyOnLoad { get; }

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;


    private NetworkStream stream;
    [SerializeField]
    private string username, password, channelName;

    [SerializeField]
    private MessageInterpreter messageInterpreter;

    void Start()
    {
        Connect();
    }

    void Update()
    {
        if (!twitchClient.Connected)
        {
            Debug.Log("twitch client not connected");
            Connect();
        }

        ReadChat();
    }
    
    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username.ToLower());
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName.ToLower());
        writer.WriteLine("CAP REQ :twitch.tv/tags"); 
        writer.WriteLine("CAP REQ :twitch.tv/commands");
        writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.Flush();
    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0) { 
            var message = reader.ReadLine(); //Read in the current message
            if (message.Contains("PRIVMSG")) // when a user send a message
            {
                string tags = message.Split(' ')[0];
                message = message.Remove(0, message.IndexOf(' '));
                string[] messageSplit = message.Split(new string[] {" :!"}, StringSplitOptions.None);

                if (message == messageSplit[0])
                    return;

                message = messageSplit[messageSplit.Length - 1];

                //Get the users name by splitting it from the string
                var splitPoint = messageSplit[0].IndexOf("!", 1);
                var chatName = messageSplit[0].Substring(0, splitPoint);
                string messageSenderUsername = chatName.Substring(2);

                //Get the users message by splitting it from the string
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                print(message);

                //Run the instructions to control the game!
                messageInterpreter.GetMessage(message, messageSenderUsername, tags);
            }
            else if (message.Contains("PING"))
            {
                writer.WriteLine("PONG");
                writer.Flush();
                return;
            }
            else if (message.Contains("355")) //a person join the chat
            {

            }
        }
    }
    public void SendMessageToTwitchChat(string message, string username = null)
    {
        try
        {
            if(username == null)
                SendIrcMessage(":" + username.ToLower() + "!" + username.ToLower() + "@" + username.ToLower() + ".tmi.twitch.tv PRIVMSG #" + channelName.ToLower() + " :" + message);
            else
            {
                string msg = ":" + username.ToLower() + "!" + username.ToLower() + "@" + username.ToLower() + ".tmi.twitch.tv PRIVMSG #" + channelName.ToLower() + " : @" + username + " " + message;
                SendIrcMessage(msg);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void SendIrcMessage(string message)
    {
        try
        {
            writer.WriteLine(message);
            writer.Flush();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
