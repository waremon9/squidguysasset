using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUsername : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private TMP_Text usernameText;

    [SerializeField] private Camera cameraToLookAt;
    void Start()
    {
        cameraToLookAt = Camera.main;
        usernameText.text = player.Name;
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }

    public void SetUsernameColor(Color newColor)
    {
        usernameText.color = newColor;
    }
    public void SetUsernameText(string t)
    {
        usernameText.text = t;
    }
}
