using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Events/Data loaded")]
public class DataLoadedEvent : ScriptableObject
{
    [SerializeField]
    private List<DataLoadedListener> _listeners;

    public void Raise(GameData gameData)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(gameData);
        }
    }

    public void RegisterListener(DataLoadedListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(DataLoadedListener listener)
    {
        _listeners.Remove(listener);
    }
}
