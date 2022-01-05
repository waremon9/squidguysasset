using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player Win Data")]
public class PlayerWinData : ScriptableObject
{
    public bool Draw;
    public string PlayerWinUsername;
    public Color ColorUsername;
}
