using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{




    [SerializeField]
    private CooldownTimer timer;
    [SerializeField]
    private GameObject playerUsernameTextPrefab;
    [SerializeField]
    private GameObject playerUsernameCanvas;

    public TMP_Text stateText;
    public TMP_Text cooldownText;




    
    private int cooldownSec;
    private List<GameObject> playerUsernameTextList = new List<GameObject>();

    private void Start()
    {
        InitTimerUI();
    }
    private void Update()
    {
        if(cooldownText) UpdateTimerUI();
    }

    private void InitTimerUI()
    {
        cooldownSec = Mathf.RoundToInt(timer.GetCD() / 1000);
        timer.StartCooldown();
        cooldownText.text = cooldownSec.ToString();

    }
    private void UpdateTimerUI()
    {
        cooldownSec = Mathf.CeilToInt(timer.GetTimeLeft()/1000);
        stateText.text = timer.phase;
        cooldownText.text = cooldownSec.ToString();
        cooldownText.gameObject.SetActive(GameManager.Instance.gameState.gState != GState.Action);

        ChangeColor();

        if (timer.IsCooldownDone())
        {
            timer.StopCooldown();
           
            
        }

    }

    private void ChangeColor()
    {
        float T = Mathf.Pow( timer.GetTimeLeft() / timer.GetCD(),2);
        Vector2 Bezier = MathUtilities.Bezier2(new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 1), T);
        
        
        stateText.color = new Color(1, Bezier.x, Bezier.y, 1);
        cooldownText.color = new Color(1, Bezier.x, Bezier.y, 1);
        
       
    }

    public void OnPlayerJoin(string playerUsername)
    {
        GameObject playerTextUsername = Instantiate(playerUsernameTextPrefab, playerUsernameCanvas.transform);
        playerTextUsername.GetComponent<TMP_Text>().text = playerUsername;

        playerUsernameTextList.Add(playerTextUsername);
    }
}
