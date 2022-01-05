using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MySingleton<HazardManager>
{
    public override bool DoDestroyOnLoad { get; }

    [SerializeField] private CanonSettings canon;
    [SerializeField] private PushCanon CanonPrefab;
    private List<PushCanon> AllCanons = new List<PushCanon>();

    public Transform hazardParent;

    public bool hazardFinishedPlaying = true;
    public bool GetHazardFinishedPlaying(){ return hazardFinishedPlaying; }

    /// <summary> Get list of canon. </summary>
    public List<PushCanon> GetCanons() { return AllCanons; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            CreateCanonRandomPosition();
        }
    }

    /// <summary>
    /// Add a new canon to the list.
    /// </summary>
    /// <param name="pc">Canon to add.</param>
    public void AddCanon(PushCanon pc)
    {
        AllCanons.Add(pc);
    }

    /// <summary>
    /// Reset the canon list.
    /// </summary>
    public void ClearCanon()
    {
        AllCanons.Clear();
    }

    /// <summary>
    /// Get the canon at the given coordinates
    /// </summary>
    /// <param name="coord">canon's coordinates</param>
    /// <returns>Canon if found. null otherwise.</returns>
    public PushCanon GetCanonAtPosition(Vector2Int coord)
    {
        return GameManager.Instance.AllCanons.Find((pc) => { return pc.Coordinates == coord; });
    }

    private int bulletsActive;
    /// <summary>
    /// Activate all hazards.
    /// Canon will shoot.
    /// </summary>
    public void ActivateAllHazards()
    {
        hazardFinishedPlaying = false;

        foreach (PushCanon pushCanon in AllCanons)
        {
            pushCanon.Activate();
        }
        bulletsActive = AllCanons.Count;
    }

    public void BulletDestroyed()
    {
        bulletsActive--;
        if(bulletsActive == 0)
        {
            hazardFinishedPlaying = true;
        }
    }

    /// <summary>
    /// Instantiate a new canon and put it at a random position.
    /// </summary>
    public void CreateCanonRandomPosition()
    {
        PushCanon pc = Instantiate(CanonPrefab, hazardParent);
        System.Tuple<Direction, Vector2Int> canonPosition = ArenaManager.Instance.GetRandomBorderForCanon();
        pc.SetCanonPosition(canonPosition.Item2, canonPosition.Item1);
        AllCanons.Add(pc);
    }

    public void CreateCannonFromGameData(GameData gd)
    {
        AllCanons.Clear();
        foreach (CannonData cd in gd.cannonsData)
        {
            PushCanon pc = Instantiate(CanonPrefab, hazardParent);
            pc.SetCanonPosition(cd.coordinate, cd.dir);
            AllCanons.Add(pc);
        }
    }
}
