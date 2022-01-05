using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MySingleton<PlayerManager>
{
    [SerializeField] List<Player> AllPlayers = new List<Player>();//all the player in the game, alive or dead.
    private List<Player> PlayerAlive = new List<Player>();//player that are still alive, used to check end game

    [SerializeField] GameEndEvent gameEndEvent;
    
    public override bool DoDestroyOnLoad { get; }
    
    public int PlayerAmount //amount of player in the game
    {
        get { return AllPlayers.Count; }
    }

    public List<Player> GetPlayers()
    {
        return AllPlayers;
    }

    /// <summary>
    /// Add a new player to the game by adding it to the List of player.
    /// </summary>
    /// <param name="p">Player to add.</param>
    public void AddPlayer(Player p)
    {
        if (AllPlayers.Contains(p)) return;
        AllPlayers.Add(p);
        PlayerAlive.Add(p);
    }

    public Player GetPlayerByIndex(int index)
    {
        if (index >= AllPlayers.Count) return null;
        return AllPlayers[index];
    }

    public Player GetPlayerByID(int ID)
    {
        return AllPlayers.Find((p) => { return p.Identifiant == ID; });
    }
    
    public Player GetPlayerByName(string name)
    {
        return AllPlayers.Find((p) => { return p.Name == name; });
    }

    /// <summary>
    /// Check if a player is at the given coordinate.
    /// </summary>
    /// <param name="Coord">Coordinate to check.</param>
    /// <returns>Return player if found, null otherwise.</returns>
    public Player GetPlayerAtPosition(Vector2 Coord)
    {
        return AllPlayers.Find((p) => { return p.Coordinates == Coord; });
    }

    /// <summary>
    /// Check if a player is at the given coordinate and isn't player in param.
    /// </summary>
    /// <param name="Coord">Coordinate to check.</param>
    /// <param name="player">Player to ignore.</param>
    /// <returns>Return player if found, null otherwise.</returns>
    public Player GetPlayerAtPosition(Vector2 Coord, Player player)
    {
        return AllPlayers.Find((p) => { return p != player && !p.IsDead && p.Coordinates == Coord; });
    }

    /// <summary>
    /// Clear the AllPlayer and PlayerAlive list.
    /// </summary>
    public void Clear()
    {
        AllPlayers.Clear();
        PlayerAlive.Clear();
    }

    /// <summary>
    /// Reset player last and future coordinates to actual coordinates.
    /// </summary>
    public void SavePlayersPosition()
    {
        foreach (Player player in AllPlayers)
        {
            player.LastCoordinates = player.Coordinates;
            player.FuturCoordinates = player.Coordinates;
        }
    }

    /// <summary>
    /// Will call movement coroutine of all the player.
    /// </summary>
    /// <remarks>Coroutine end movement from all player ended.</remarks>
    public IEnumerator MovePlayersCoroutine()
    {
        List<MoveComponent> AllMovingPlayerMovComp = new List<MoveComponent>();//state of the moving coroutine is in the movement component
        foreach (Player p in AllPlayers)
        {
            if (p.FuturCoordinates == p.Coordinates) continue;//don't move so no coroutine
            StartCoroutine(p.MoveComp.MoveVisualCoroutine());
            AllMovingPlayerMovComp.Add(p.MoveComp);
        }

        //don't leave the loop while all the movement aren't finished
        while (AllMovingPlayerMovComp.Count > 0)
        {
            for (int i = AllMovingPlayerMovComp.Count - 1; i >= 0; i--)
            {
                if (!AllMovingPlayerMovComp[i].IsCoroutineMoveRunning)//check if the movement coroutine ended for this player
                {
                    AllMovingPlayerMovComp.RemoveAt(i);
                }
            }

            yield return new WaitForSeconds(0.1f);//no need to check every frame
        }
    }

    ///<summary>
    /// Check if player in param and a player on adjacent platform will collide when moving.
    /// </summary>
    /// <param name="player">The player to check with adjacent platform.</param>
    ///<returns>Player we collide with. null otherwise.</returns>
    public Player CheckCollisionDuringMove(Player player)
    {
        foreach (Player other in AllPlayers)
        {
            if (player == other || other.IsDead) continue;
            if (player.FuturCoordinates == other.Coordinates &&
                other.FuturCoordinates == player.Coordinates)
            {
                return other;
            }
        }
        return null;
    }

    /// <summary>
    /// Check if a player will go on a platform that another player want to go too.
    /// </summary>
    /// <param name="player">The player to check his destination.</param>
    /// <returns>Players coming. Can be empty.</returns>
    public List<Player> CheckCollisionOnDestinationPlatformWithMovingPlayer(Player player)
    {
        List<Player> result = new List<Player>();
        foreach (Player other in AllPlayers)
        {
            if (player == other || other.IsDead) continue;
            if(player.FuturCoordinates == other.FuturCoordinates && other.MoveComp.CumulMove != Vector2Int.zero)
            {
                result.Add(other);
            }
        }

        return result;
    }
    
    /// <summary>
    /// Check if a player will go on a platform already occupied.
    /// </summary>
    /// <param name="player">The player to check his destination.</param>
    /// <returns>Player present. null otherwise.</returns>
    public Player CheckCollisionOnDestinationPlatformWithAlreadyPresentPlayer(Player player)
    {
        foreach (Player other in AllPlayers)
        {
            if (player == other || other.IsDead) continue;
            if(player.FuturCoordinates == other.Coordinates && other.MoveComp.CumulMove == Vector2Int.zero)
            {
                return other;
            }
        }
        return null;
    }

    /// <summary>
    /// When a player move to another coordinate, check if the platform exist. Kill player if no platform.
    /// </summary>
    /// <param name="player">Player to check.</param>
    /// <returns>True if the player is dead.</returns>
    public bool CheckPlayerDie(Player player)
    {
        Platform platform = ArenaManager.Instance.GetPlatformAtCoord(player.Coordinates);

        if (!platform)
        {
            player.Die();
            PlayerAlive.Remove(player);

            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if the game ended. Game end when 1 or 0 player are still alive.
    /// </summary>
    public void CheckEndGame()
    {
        if (PlayerAlive.Count == 1)
        {
            gameEndEvent.Raise(PlayerAlive[0]);
        }
        else if (PlayerAlive.Count == 0)
        {
            gameEndEvent.Raise(null);
        }
    }
}
