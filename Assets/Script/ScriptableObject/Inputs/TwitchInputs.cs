using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inputs/Twitch Inputs")]
public class TwitchInputs : ScriptableObject
{
    public List<GameInputs> gameInputs = new List<GameInputs>();
}
