using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioObject : ScriptableObject
{
    public abstract void Play(AudioSource source, float? Vol = null);
}
