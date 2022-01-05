using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Single clip")]
public class AudioSingleClip : AudioObject
{
    [SerializeField] private AudioClip Clip;
    [SerializeField, Range(0,1)] private float Volume;
    public override void Play(AudioSource source, float? Vol = null)
    {
        source.clip = Clip;
        source.volume = Vol != null ? (float)Vol : Volume;
        source.Play();
    }
}
