using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState")]
public class GameState : ScriptableObject
{
    public GState gState;
}

public enum GState
{
    Join,
    Vote,
    Action,
    Win
}