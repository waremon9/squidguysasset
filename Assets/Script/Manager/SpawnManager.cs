using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MySingleton<SpawnManager>
{
    public override bool DoDestroyOnLoad { get; }

    public GameObject playerParent;

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject PlayerIAPrefab;
    [SerializeField] private bool BotPlayerUseIA;

    private int IdIncrement = 0;

    protected override void Awake()
    {
        base.Awake();
        PlayerManager.Instance.Clear();//to put somewhere else
    }

    /// <summary> Spawn a test player. </summary>
    public void SpawnNewPlayer(string Name)
    {
        Platform SpawnPlatform = ArenaManager.Instance.GetPossibleSpawnPoint();
        if (SpawnPlatform == null) return ;

        Vector3 Position =  SpawnPlatform.transform.position + Vector3.up * PlayerPrefab.GetComponent<CombinedBounds>().GetCombinedBounds().size.y / 2;

        Player player;
        if (BotPlayerUseIA)
        {
            Debug.LogWarning("IA player");
            player = Instantiate(PlayerIAPrefab, Position, Quaternion.identity, playerParent.transform).GetComponent<Player>();
        }
        else
        {
            Debug.LogWarning("normal player");
            player = Instantiate(PlayerPrefab, Position, Quaternion.identity, playerParent.transform).GetComponent<Player>();
        }

        player.Coordinates = SpawnPlatform.Coordinates;
        player.Name = Name;
        player.name = Name;
        player.Identifiant = IdIncrement++;

        PlayerManager.Instance.AddPlayer(player.GetComponent<Player>());
    }

    /// <summary>
    /// Spawn a new player on a random, non-border, empty platform.
    /// If BotPlayerUseIA in editor is True, player will randomly move each turn.
    /// </summary>
    /// <param name="Name">player's name.</param>
    /// <param name="usernameColor">Color of the player's name.</param>
    public void SpawnNewPlayer(string Name, Color usernameColor)
    {
        Platform SpawnPlatform = ArenaManager.Instance.GetPossibleSpawnPoint();
        if (SpawnPlatform == null) return;

        Vector3 Position = SpawnPlatform.transform.position + Vector3.up * PlayerPrefab.GetComponent<CombinedBounds>().GetCombinedBounds().size.y / 2;

        Player player;
        if (BotPlayerUseIA)
        {
            Debug.LogWarning("IA player");
            player = Instantiate(PlayerIAPrefab, Position, Quaternion.identity, playerParent.transform).GetComponent<Player>();
        }
        else
        {
            Debug.LogWarning("normal player");
            player = Instantiate(PlayerPrefab, Position, Quaternion.identity, playerParent.transform).GetComponent<Player>();
        }

        player.Coordinates = SpawnPlatform.Coordinates;
        player.Name = Name;
        player.name = Name;
        player.Identifiant = IdIncrement++;
        player.playerUsername.SetUsernameColor(usernameColor);

        PlayerManager.Instance.AddPlayer(player.GetComponent<Player>()); 
    }

    /// <summary>
    /// Spawn multiple player.
    /// </summary>
    /// <param name="Names">All the player to spawn.</param>
    public void SpawnNewPlayer(Dictionary<string, Color> Names)
    {
        foreach (var player in Names)
        {
            SpawnNewPlayer(player.Key, player.Value);
        }
    }

    public void SpawnPlayerFromGameData(GameData gd)
    {
        PlayerManager.Instance.Clear();

        foreach (PlayerData pd in gd.playerDatas)
        {
            Vector3 Position = ArenaManager.Instance.GetPositionAtCoordinates(pd.position);

            Player player = Instantiate(PlayerPrefab, Position, Quaternion.identity, playerParent.transform).GetComponent<Player>();

            player.Coordinates = pd.position;
            player.Name = pd.username;
            player.name = pd.username;
            player.Identifiant = IdIncrement++;
            player.playerUsername.SetUsernameColor(pd.usernameColor);

            PlayerManager.Instance.AddPlayer(player.GetComponent<Player>());
        }
    }
}
