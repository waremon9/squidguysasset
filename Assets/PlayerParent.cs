using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SpawnManager.Instance.playerParent = gameObject;
    }
}
