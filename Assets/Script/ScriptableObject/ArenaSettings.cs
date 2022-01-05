using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Arena Settings")]
public class ArenaSettings : ScriptableObject
{
    /// <summary> Number of platforms for each player </summary>
    public float PlayerPlatformRatio;
    /// <summary> Durability of the platform </summary>
    public int PlatformDurability;
    /// <summary>Number of player for each canon. 0 means no canon.</summary>
    public int CanonPlayerRatio = 0;
}
