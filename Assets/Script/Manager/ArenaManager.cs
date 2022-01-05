using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArenaManager : MySingleton<ArenaManager>
{
    [SerializeField] private List<Platform> AllPlatforms = new List<Platform>();
    [SerializeField] private Camera camera;

    public ArenaSettings ArenaSettings;
    public List<List<Platform>> MatrixPlatform;

    [SerializeField] private GameObject PlatformPrefab;
    public float TileSize = 2.2f;
    [SerializeField] private float ArenaHeight = 5;

    public GameObject platformsParent;

    public int Dimension;
    
    public override bool DoDestroyOnLoad { get; }

    /// <summary>
    /// Platform got destroyed, update AllPlatforms list and MatrixPlatform.
    /// </summary>
    /// <param name="platform">The destroyed platform.</param>
    public void PlatformDestroyed(Platform platform)
    {
        AllPlatforms.Remove(platform);
        MatrixPlatform[platform.Coordinates.x][platform.Coordinates.y] = null;
    }

    /// <summary>
    /// Create an arena according to the number of player and the ArenaSettings.
    /// </summary>
    /// <param name="QtePlayer">Number of player for the game.</param>
    public void CreateArena(int QtePlayer)
    {
        camera = Camera.main;
        
        AllPlatforms.Clear();//Clear all platform list;
        
        //calculate the dimension of the arena.
        // --> Ceil ( Sqrt ( NumberOfPlayer * PlayerToPlatformRation ) ) + 2
        float QtePlatformAprox = QtePlayer * ArenaSettings.PlayerPlatformRatio;
        Dimension = Mathf.CeilToInt(Mathf.Sqrt(QtePlatformAprox)) + 2;// +2 for side that are empty so off limit player's coordinates don't overflow the list
        camera.transform.position = new Vector3(0, 10 + 1.5f * Dimension, 0);//height of the camera so everything is visible
        MatrixPlatform = new List<List<Platform>>();

        for (int x = 0; x < Dimension; x++)
        {
            List<Platform> TmpList = new List<Platform>();

            for(int y = 0; y < Dimension; y++)
            {
                if (x == 0 || x == Dimension - 1 || y == 0 || y == Dimension - 1)
                {
                    TmpList.Add(null); //borders are empty
                }
                else
                {
                    //Instantiate new platform.
                    Vector3 Position = new Vector3((-TileSize * Dimension + TileSize) / 2 + TileSize * x, ArenaHeight, (-TileSize * Dimension + TileSize) / 2 + TileSize * y);
                    Platform TmpPlatform = Instantiate<GameObject>(PlatformPrefab, Position, Quaternion.identity, platformsParent.transform).GetComponent<Platform>();
                    TmpPlatform.MaxDurability = ArenaSettings.PlatformDurability; //Create platform with durability according to ArenaSettings.
                    TmpPlatform.durability = ArenaSettings.PlatformDurability;
                    TmpPlatform.Coordinates = new Vector2Int(x, y);

                    
                    TmpList.Add(TmpPlatform);
                    AllPlatforms.Add(TmpPlatform);
                }
            }
            MatrixPlatform.Add(TmpList);
        }

        if(ArenaSettings.CanonPlayerRatio > 0)
        {
            int qteCanon = Mathf.CeilToInt((float)QtePlayer / ArenaSettings.CanonPlayerRatio);
            for(int i = 0; i<qteCanon; i++)
            {
                HazardManager.Instance.CreateCanonRandomPosition();
            }
        }
    }

    public void CreateArena(GameData gd)
    {
        camera = Camera.main;

        AllPlatforms.Clear();//Clear all platform list;

        Dimension = gd.arenaDimension;

        camera.transform.position = new Vector3(0, 10 + 1.5f * Dimension, 0);//height of the camera so everything is visible
        
        //empty matrix
        MatrixPlatform = new List<List<Platform>>();
        for (int x = 0; x < Dimension; x++)
        {
            List<Platform> TmpList = new List<Platform>();

            for (int y = 0; y < Dimension; y++)
            {
                TmpList.Add(null); //borders are empty
            }

            MatrixPlatform.Add(TmpList);
        }

        foreach(PlatformData pd in gd.platformDatas)
        {
            //Instantiate new platform.
            Vector3 Position = new Vector3((-TileSize * Dimension + TileSize) / 2 + TileSize * pd.position.x, ArenaHeight, (-TileSize * Dimension + TileSize) / 2 + TileSize * pd.position.y);
            Platform TmpPlatform = Instantiate<GameObject>(PlatformPrefab, Position, Quaternion.identity, platformsParent.transform).GetComponent<Platform>();
            TmpPlatform.MaxDurability = gd.maxDurabilityPlatforms;
            TmpPlatform.durability = pd.durability;
            MatrixPlatform[pd.position.x][pd.position.y] = TmpPlatform;
            TmpPlatform.UpdateColor();
            AllPlatforms.Add(TmpPlatform);
        }
    }

    /// <summary>
    /// Get platform without player on it and not at the border of the arena.
    /// </summary>
    /// <returns>Platform available for spawn. null otherwise.</returns>
    public Platform GetPossibleSpawnPoint()
    {
        List<Platform> platforms = new List<Platform>();
        //Get all non-border platforms.
        foreach(Platform p in AllPlatforms)
        {
            if (p.Coordinates.x >= 2 && p.Coordinates.x < Dimension-2 &&
                p.Coordinates.y >= 2 && p.Coordinates.y < Dimension - 2)
            {
                platforms.Add(p);
            }
        }
        //remove platforms with player already on them.
        foreach(Player player in PlayerManager.Instance.GetPlayers())
        {
            Platform p = platforms.Find((platform) => { return platform.Coordinates == player.Coordinates; });
            if(p != null)
            {
                platforms.Remove(p);
            }
        }
        if (platforms.Count == 0)
        {
            Debug.LogError("no platform available for spawn. " + gameObject.name);
            return null;
        }
        return platforms[Random.Range(0, platforms.Count)];//return a random platform
    }

    /// <summary>
    /// Turn platform coordinate into world position with player height offset.
    /// </summary>
    /// <param name="Coord">Platform's coordinate.</param>
    /// <returns>World position of the platform.</returns>
    public Vector3 GetPositionAtCoordinates(Vector2 Coord)
    {
        return new Vector3((-TileSize * Dimension + TileSize) / 2 + TileSize * Coord.x,
            ArenaHeight + 1.1f/*1.1 is half the height of a platform*/,
            (-TileSize * Dimension + TileSize) / 2 + TileSize * Coord.y);
    }

    /// <summary>
    /// Get the platform at the given coordinate.
    /// </summary>
    /// <param name="Coord">Platform's coordinate.</param>
    /// <returns>Platform if not destroyed. null otherwise.</returns>
    /// <remarks>Not underflow/overflow protected.</remarks>
    public Platform GetPlatformAtCoord(Vector2Int Coord)
    {
        return MatrixPlatform[Coord.x][Coord.y];
    }

    /// <summary>
    /// Damage the platform at the given coordinate.
    /// </summary>
    /// <param name="Coord">Platform's coordinate.</param>
    public void DamagePlatformAtCoordinate(Vector2Int Coord)
    {
        Platform p = GetPlatformAtCoord(Coord);
        if (p)
        {
            p.ReduceDurability();
        }
    }

    /// <summary>
    /// Damage all platforms with players on them that are not performing a move command this turn.
    /// </summary>
    public void DamageAllPlatformWithPlayerNotMoving()
    {
        foreach (Player p in PlayerManager.Instance.GetPlayers())
        {
            if (!p.IsDead && p.MoveComp.CumulMove == Vector2Int.zero)
            {
                DamagePlatformAtCoordinate(p.Coordinates);
                PlayerManager.Instance.CheckPlayerDie(p);
            }
        }
    }

    /// <summary>
    /// Get a random canon placement. Canoncan only be placed on the edge of the arena and not on corner.
    /// </summary>
    /// <returns>Tuple with direction and random coordinate.</returns>
    public Tuple<Direction,Vector2Int> GetRandomBorderForCanon()
    {
        Array A = Enum.GetValues(typeof(Direction));

        Tuple<Direction, Vector2Int> res;

        do
        {
            Direction dir = (Direction) A.GetValue(UnityEngine.Random.Range(0, A.Length));

            switch (dir)
            {
                case Direction.Down:
                    res = new Tuple<Direction, Vector2Int>(dir, new Vector2Int(Random.Range(1, Dimension - 2), Dimension - 1));
                    break;
                case Direction.Up:
                    res = new Tuple<Direction, Vector2Int>(dir, new Vector2Int(Random.Range(1, Dimension - 2), 0));
                    break;
                case Direction.Left:
                    res = new Tuple<Direction, Vector2Int>(dir,
                        new Vector2Int(Dimension - 1, Random.Range(1, Dimension - 2)));
                    break;
                case Direction.Right:
                    res = new Tuple<Direction, Vector2Int>(dir, new Vector2Int(0, Random.Range(1, Dimension - 2)));
                    break;
                default:
                    Debug.Log("bad direction " + name);
                    return null;
            }
        } while (HazardManager.Instance.GetCanonAtPosition(res.Item2));

        return res;
    }
}
