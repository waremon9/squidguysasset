using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MySingleton<AudioManager>
{
    public override bool DoDestroyOnLoad { get; }

    [SerializeField] private AudioSource Source;

    [SerializeField] private AudioObject PlatformBreakAudioObject;

    private void Start()
    {
        GetVolume();
    }

    public void PlatformBreak()
    {
        //PlatformBreakAudioObject.Play(Source, GetVolume());
    }

    public float? GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume");
    }

    public void SetVolume(float Volume)
    {
        Volume = Mathf.Clamp(Volume, 0, 1);
        PlayerPrefs.SetFloat("Volume", Volume);
    }


}
