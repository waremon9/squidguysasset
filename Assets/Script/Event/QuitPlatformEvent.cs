using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/Quit platform")]
public class QuitPlatformEvent : ScriptableObject
{
    [SerializeField]
    private List<QuitPlatformEventListener> _listeners;

    public void Raise(Vector2Int Coord)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(Coord);
        }
    }

    public void RegisterListener(QuitPlatformEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(QuitPlatformEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
