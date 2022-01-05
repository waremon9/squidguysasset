using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformBreakEventListener : MonoBehaviour
{
    [SerializeField]
    private PlatformBreakEvent _event;

    [SerializeField]
    private UnityEvent<Platform> _onEventRaised;

    public void OnEventRaised(Platform p)
    {
        _onEventRaised.Invoke(p);
    }

    private void OnEnable()
    {
        _event.RegisterListener(this);
    }

    private void OnDisable()
    {
        _event.UnregisterListener(this);
    }
}
