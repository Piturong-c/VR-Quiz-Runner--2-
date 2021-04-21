using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager self;
    public AudioClip[] audios;
    public AudioSource source;
    void Awake()
    {
        if(self == null)
            self = this;
        
        source = GetComponent<AudioSource>();
        audios = Resources.LoadAll<AudioClip>("soundFX/");
    }

    public void PlayAudio(string name)
    {
        for (int i = 0; i < audios.Length; i++)
        {
            if (name == audios[i].name)
            {
                source.PlayOneShot(audios[i]);
                Debug.LogWarning($"Found audio {name}!");
                return;
            }
        }

        Debug.LogWarning("Not found other!");
    }
}
