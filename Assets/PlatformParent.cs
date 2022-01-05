using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParent : MonoBehaviour
{
    private void Awake()
    {
        ArenaManager.Instance.platformsParent = gameObject;
    }
}
