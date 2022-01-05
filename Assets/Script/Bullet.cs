using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;
    public Direction direction;

    public void Start()
    {
        lifeTime = ((ArenaManager.Instance.Dimension + 1) * ArenaManager.Instance.TileSize) / speed;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        
        transform.position += transform.forward * speed * Time.deltaTime; // "F*** TRANSLATE" - Le sel de Thomas, 12 decembre 2021.
    }

    private void OnDestroy()
    {
        HazardManager.Instance.BulletDestroyed();
    }
}
