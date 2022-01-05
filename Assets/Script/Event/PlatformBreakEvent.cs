using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Platform break")]
public class PlatformBreakEvent : ScriptableObject
{
    [SerializeField]
    private List<PlatformBreakEventListener> _listeners;

    public void Raise(Platform p)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(p);
        }
    }

    public void RegisterListener(PlatformBreakEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(PlatformBreakEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
