using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Start Game")]
public class StartGame : ScriptableObject
{
    [SerializeField]
    private List<StartGameListener> _listeners;

    public void Raise(string messageID)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(messageID);
        }
    }

    public void RegisterListener(StartGameListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(StartGameListener listener)
    {
        _listeners.Remove(listener);
    }
}
