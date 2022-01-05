using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="List/Player")]
public class ListUsernames : ScriptableObject
{
    [SerializeField]
    public Dictionary<string, Color> playersUsernames = new Dictionary<string, Color>();

    private void OnApplicationQuit()
    {
        playersUsernames.Clear();
    }

    public Color GetPlayerColor(string playerUsername) {
        Color colorToReturn;
        playersUsernames.TryGetValue(playerUsername, out colorToReturn);
        return colorToReturn;
    }
}
