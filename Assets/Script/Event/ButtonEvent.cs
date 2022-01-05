using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Button Click")]
public class ButtonEvent : ScriptableObject
{
    [SerializeField]
    private List<ButtonEventListener> _listeners;

    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(ButtonEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(ButtonEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
