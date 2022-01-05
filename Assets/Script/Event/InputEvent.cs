using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Input")]
public class InputEvent : ScriptableObject
{
    [SerializeField]
    private List<InputEventListener> _listeners;

    public void Raise(Player p, CommandType CT)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(p, CT);
        }
    }

    public void RegisterListener(InputEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(InputEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
