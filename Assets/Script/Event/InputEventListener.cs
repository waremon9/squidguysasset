using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputEventListener : MonoBehaviour
{
    [SerializeField]
    private InputEvent _event;

    [SerializeField] private UnityEvent<Player,CommandType> _onEventRaised;

    public void OnEventRaised(Player p, CommandType CT)
    {
        _onEventRaised.Invoke(p, CT);
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
