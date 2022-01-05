using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Player join")]
public class PlayerJoinEvent : ScriptableObject
{
    [SerializeField]
    private List<PlayerJoinEventListener> _listeners;

    public void Raise(string Name, Color usernameColor)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(Name, usernameColor);
        }
    }

    public void RegisterListener(PlayerJoinEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(PlayerJoinEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
