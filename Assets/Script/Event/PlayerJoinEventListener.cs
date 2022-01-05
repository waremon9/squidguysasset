using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJoinEventListener : MonoBehaviour
{
    [SerializeField]
    private PlayerJoinEvent _event;

    [SerializeField]
    private UnityEvent<string, Color> _onEventRaised;

    public void OnEventRaised(string Name, Color usernameColor)
    {
        _onEventRaised.Invoke(Name, usernameColor);
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
