using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventListener : MonoBehaviour
{
    [SerializeField]
    private ButtonEvent _event;

    [SerializeField]
    private UnityEvent _onEventRaised;

    public void OnEventRaised()
    {
        _onEventRaised.Invoke();
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
