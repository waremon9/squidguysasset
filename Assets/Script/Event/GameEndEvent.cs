using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game end")]
public class GameEndEvent : ScriptableObject
{
    [SerializeField]
    private List<GameEndEventListener> _listeners;

    public void Raise(Player p)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(p);
        }
    }

    public void RegisterListener(GameEndEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(GameEndEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
