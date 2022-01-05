using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCanon : MonoBehaviour
{
    [SerializeField] private Vector2Int coordinates;
    public Vector2Int Coordinates
    {
        get { return coordinates; }
        set { coordinates = value; }
    }
    
    private Direction direction;
    [SerializeField] private CombinedBounds combinedBounds;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject BulletSpawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Activate();
        }
    }

    public void SetCanonPosition(Vector2Int coord, Direction dir)
    {
        float halfHeight = combinedBounds.GetCombinedBounds().extents.y;
        Vector3 position = ArenaManager.Instance.GetPositionAtCoordinates(coord);
        transform.position = position - halfHeight * Vector3.up;

        switch (dir)
        {
            case Direction.Up:
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.LookRotation(Vector3.back);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.LookRotation(Vector3.right);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.LookRotation(Vector3.left);
                break;
            default:
                Debug.Log("bad direction " + name);
                break;
        }

        direction = dir;
        Coordinates = coord;
    }

    public void Activate()
    {
        Bullet b = Instantiate(bulletPrefab, BulletSpawnPoint.transform.position, transform.localRotation);
        b.transform.localScale = Vector3.one * BulletSpawnPoint.GetComponent<Renderer>().bounds.size.x;
        b.direction = direction;
    }

    public CannonData GetCannonData()
    {
        CannonData cData = new CannonData(coordinates, direction);
        return cData;
    }
}
