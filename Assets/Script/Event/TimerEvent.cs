using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Timer")]
public class TimerEvent : ScriptableObject
{
    [SerializeField]
    private List<TimerEventListener> _listeners;

    public void Raise(GState state)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(state);
        }
    }

    public void RegisterListener(TimerEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(TimerEventListener listener)
    {
        _listeners.Remove(listener);
    }

}