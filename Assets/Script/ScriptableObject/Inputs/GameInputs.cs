using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inputs/Game Inputs")]
public class GameInputs : ScriptableObject
{
    public CommandType CType;

    public List<string> commandText = new List<string>();
}
