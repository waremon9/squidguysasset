using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuitPlatformEventListener : MonoBehaviour
{
    [SerializeField]
    private QuitPlatformEvent _event;

    [SerializeField]
    private UnityEvent<Vector2Int> _onEventRaised;

    public void OnEventRaised(Vector2Int Coord)
    {
        _onEventRaised.Invoke(Coord);
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
