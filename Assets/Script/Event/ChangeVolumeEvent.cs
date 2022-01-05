using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Change Volume")]
public class ChangeVolumeEvent : ScriptableObject
{
    [SerializeField]
    private List<ChangeVolumeListener> _listeners;

    public void Raise(float newVolume)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(newVolume);
        }
    }

    public void RegisterListener(ChangeVolumeListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(ChangeVolumeListener listener)
    {
        _listeners.Remove(listener);
    }
}
