using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioBite
{
    private AudioSource source;
    [SerializeField]
    private AudioClip audio;
    [SerializeField, Range(0f, 1f)]
    private float volume;
    [SerializeField, Range(0.1f, 3f)]
    private float pitch;
    public void Play() => source?.Play();

    public void Init(GameObject gameObject) {
        if (gameObject == null) 
            return;
        if (audio == null)
            return;

        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
        source.volume = volume;
        source.pitch = pitch;
        source.clip = audio;
       
    }
    public float GetClipLength() => audio.length;
    
}
