using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataLoadedListener : MonoBehaviour
{
    [SerializeField]
    private DataLoadedEvent _event;

    [SerializeField]
    private UnityEvent<GameData> _onEventRaised;

    public void OnEventRaised(GameData gameData)
    {
        _onEventRaised.Invoke(gameData);
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
