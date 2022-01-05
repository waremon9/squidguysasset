using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Twitch Moderators")]
public class TwitchModerator : ScriptableObject
{
    [SerializeField] private List<string> moderatorUsernames = new List<string>();

    public bool CheckIfModerator(string username)
    {
        return moderatorUsernames.Contains(username.ToLower()) ? true : false;
    }
}
