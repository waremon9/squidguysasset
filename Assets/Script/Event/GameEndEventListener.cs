using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEndEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEndEvent _event;

    [SerializeField]
    private UnityEvent<Player> _onEventRaised;

    public void OnEventRaised(Player p)
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
