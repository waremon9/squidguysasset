using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeVolumeListener : MonoBehaviour
{
    [SerializeField]
    private ChangeVolumeEvent _event;

    [SerializeField]
    private UnityEvent<float> _onEventRaised;

    public void OnEventRaised(float newVolume)
    {
        _onEventRaised.Invoke(newVolume);
    }

    private void OnEnable()
    {
        _event.RegisterListener(this);
    }

    private void OnDisable()
    {
        _event.UnregisterListener(this);
    }
}
