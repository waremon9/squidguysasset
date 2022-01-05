using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartGameListener : MonoBehaviour
{
    [SerializeField]
    private StartGame _event;

    [SerializeField]
    private UnityEvent<string> _onEventRaised;

    public void OnEventRaised(string messageID)
    {
        _onEventRaised.Invoke(messageID);
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
