using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazadParent : MonoBehaviour
{
    private void Awake()
    {
        HazardManager.Instance.hazardParent = gameObject.transform;
    }
}
