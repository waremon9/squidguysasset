using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MySingleton<GameManager>
{
    public override bool DoDestroyOnLoad { get; }

    public ArenaSettings arenaSettings;
    public GameState gameState;
    public CooldownTimer timer;
    [SerializeField] private PlayerWinData playerWinData;

    [SerializeField] private BootStrap bootstrap;
    public ListUsernames listUsernames;
    [SerializeField] private int minPlayerToStart = 2;

    public List<PushCanon> AllCanons;
        
    private void Start()
    {
        gameState.gState = GState.Join;
        timer.SetCD(gameState.gState);
    }

    private void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            EndTurn();
        }
    }

    private void SwitchState()
    {
        
        switch (gameState.gState)
        {
            case GState.Vote:
                gameState.gState = GState.Action;
                break;

            case GState.Action:
                gameState.gState = GState.Vote;
                break;

            case GState.Win:
                if (CanGoToLobby)
                {
                    gameState.gState = GState.Join;
                    StartCoroutine( bootstrap.LoadLobby());
                }
                break;
                
            default:
                break;
        }
        timer.SetCD(gameState.gState);
    }

    public void EndTurn()
    {
        SwitchState();
        if(gameState.gState == GState.Action)
        {
            StartCoroutine(EndTurnCoroutine());
        }
    }

    private IEnumerator EndTurnCoroutine()
    {
        CommandExecutor CE = CommandExecutor.Instance;
        PlayerManager PM = PlayerManager.Instance;
        
        /* The push command are done first. getting pushed by a player result in you moving away from him and damaging the platform
         * you were standing on.
         * Collision check and death check are done during move. 2 player colliding during movement will result in them going back 
         * to their original platform. If the platform they were pushed on was already destroyed, none of them will die.
         * Then the move command are done. Player not moving because they choose to or because they used their turn to push will
         * damage the platform they're currently on.
         * Same movement rule as the one for push are applied.
         */

        PM.SavePlayersPosition();//save all position before push

        CE.ExecutePushCommand();//set all the future position after the push command

        yield return StartCoroutine(PM.MovePlayersCoroutine());//visualy move player accordding to other player and collision

        PM.SavePlayersPosition();//save all position before move

        CE.ExecuteMoveCommand();//set all the future position after the move command

        IAMove();

        ArenaManager.Instance.DamageAllPlatformWithPlayerNotMoving();//damage platform of non-moving player

        yield return StartCoroutine(PM.MovePlayersCoroutine());//visualy move player accordding to other player and collision

        CE.ClearList();//clear command list, ready for next turn

        HazardManager.Instance.ActivateAllHazards();

        yield return new WaitUntil(HazardManager.Instance.GetHazardFinishedPlaying);

        PlayerManager.Instance.CheckEndGame();

        SwitchState();
    }

    private void IAMove()
    {
        foreach (Player p in PlayerManager.Instance.GetPlayers())
        {
            if(p is PlayerIA ia && !ia.IsDead)
            {
                ia.RandomMove();
            }
        }
    }

    public void StartGame(string username)
    {
        if(listUsernames.playersUsernames.Count >= minPlayerToStart)
        {
            StartCoroutine(bootstrap.LoadLevelOne());
        }
        else
        {
            MessageSender.Instance.SendMessageToTwitch("Not enough player, minimal amount: " + minPlayerToStart, username);
        }
    }

    public void Level1Loaded()
    {
        gameState.gState = GState.Vote;
        timer.SetCD(gameState.gState);
        ArenaManager.Instance.CreateArena(listUsernames.playersUsernames.Count);
        SpawnManager.Instance.SpawnNewPlayer(listUsernames.playersUsernames);
    }

    public void Level1Loaded(GameData gameData)
    {
        gameState.gState = GState.Vote;
        timer.SetCD(gameState.gState);
        ArenaManager.Instance.CreateArena(gameData);
        SpawnManager.Instance.SpawnPlayerFromGameData(gameData);
        HazardManager.Instance.CreateCannonFromGameData(gameData);
    }


    private bool CanGoToLobby;
    public void EndGameListener(Player p)
    {
        StartCoroutine(EndGamecoroutine(p));
    }


    private IEnumerator EndGamecoroutine(Player p)
    {
        CanGoToLobby = false;

        gameState.gState = GState.Win;

        StopCoroutine(EndTurnCoroutine());

        playerWinData.Draw = p == null;
        if (!playerWinData.Draw)
        {
            playerWinData.PlayerWinUsername = p.Name;
            playerWinData.ColorUsername = listUsernames.GetPlayerColor(p.Name);
        }

        yield return StartCoroutine(bootstrap.LoadWin());

        CanGoToLobby = true;
    }

    public void OnDataLoaded(GameData gameData)
    {
        StartCoroutine(bootstrap.ReloadLevelOne(gameData));
    }

    public int GetMaxPlayerAmount()
    {
        return listUsernames.playersUsernames.Count;
    }

    public async void ReadArenaSettings()
    {
        string? text1 = await GoogleSheetClient.Instance.GetCellData("A2", 2000);
        string? text2 = await GoogleSheetClient.Instance.GetCellData("A3", 2000);
        string? text3 = await GoogleSheetClient.Instance.GetCellData("A4", 2000);

        string? value1 = await GoogleSheetClient.Instance.GetCellData("B2", 2000);
        string? value2 = await GoogleSheetClient.Instance.GetCellData("B3", 2000);
        string? value3 = await GoogleSheetClient.Instance.GetCellData("B4", 2000);

        Debug.Log($"{text1} = {value1}");
        Debug.Log($"{text2} = {value2}");
        Debug.Log($"{text3} = {value3}");
    }

    public async void WriteArenaSettings()
    {
        string? value1 = await GoogleSheetClient.Instance.GetCellData("C2", 2000);
        string? value2 = await GoogleSheetClient.Instance.GetCellData("C3", 2000);
        string? value3 = await GoogleSheetClient.Instance.GetCellData("C4", 2000);

        string[] data = { value1, value2, value3 };

        await GoogleSheetClient.Instance.WriteLineData("B2:B4", 2000, data);
    }
}
