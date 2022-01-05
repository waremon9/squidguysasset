using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerEventListener : MonoBehaviour
{
    [SerializeField]
    private TimerEvent _event;

    [SerializeField]
    private UnityEvent<GState> _onEventRaised;

    public void OnEventRaised(GState state)
    {
        _onEventRaised.Invoke(state);
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
