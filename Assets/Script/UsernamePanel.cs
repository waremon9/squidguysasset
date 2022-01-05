using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UsernamePanel : MonoBehaviour
{
    [SerializeField] private GameObject playerUsernameTextPrefab;
    List<TMP_Text> playersUsernameText = new List<TMP_Text>();
    public void OnPlayerJoin(string username, Color usernameColor)
    {
        foreach (TMP_Text playerUsername in playersUsernameText)
        {
            if (playerUsername.text == username) return;
        }
        TMP_Text usernameTextTemp = Instantiate(playerUsernameTextPrefab, transform).GetComponent<TMP_Text>();
        usernameTextTemp.text = username;
        usernameTextTemp.color = usernameColor;
        playersUsernameText.Add(usernameTextTemp);
    }
}
