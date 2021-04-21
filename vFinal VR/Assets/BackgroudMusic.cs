using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudMusic : MonoBehaviour
{
    public AudioSource source;
    public float length = 0;
    public float first = 1f;
    public float last = 1f;
    void Start()
    {
        source = GetComponent<AudioSource>();
        length = source.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying)
        {
            if (source.time <= first)
            {
                source.volume += Time.deltaTime;
                source.volume = Mathf.Clamp01(source.volume);
            }
            
            if(source.time >= length - last)
            {
                source.volume -= Time.deltaTime;
            }
        }
    }
}
