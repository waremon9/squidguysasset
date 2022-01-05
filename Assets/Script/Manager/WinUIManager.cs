using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinUIManager : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private PlayerWinData playerWinData;


    [SerializeField]
    private CooldownTimer timer;

    public TMP_Text stateText;
    public TMP_Text cooldownText;





    private int cooldownSec;
    

    // Start is called before the first frame update
    void Start()
    {
        InitTimerUI();
        player.gameObject.SetActive(!playerWinData.Draw);
        if (!playerWinData.Draw)
        {
            player.playerUsername.SetUsernameColor(playerWinData.ColorUsername);
            player.playerUsername.SetUsernameText(playerWinData.PlayerWinUsername);
            player.Win();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerUI();
    }

    private void InitTimerUI()
    {
        timer.SetCD(GState.Win);
        cooldownSec = Mathf.RoundToInt(timer.GetCD() / 1000);
        cooldownText.text = cooldownSec.ToString();

    }
    private void UpdateTimerUI()
    {
        cooldownSec = Mathf.CeilToInt(timer.GetTimeLeft() / 1000);
        stateText.text = timer.phase;
        cooldownText.text = cooldownSec.ToString();
        


        if (timer.IsCooldownDone())
        {
            timer.StopCooldown();

            
        }

    }
}
