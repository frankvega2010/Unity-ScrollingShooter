using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    public AudioSource song;

    private void Start()
    {
        song = GetComponent<AudioSource>();
        song.Play();
    }

    public void playMenuMusic()
    {
        if (!song.isPlaying)
        {
            song.Play();
        }
    }

    public void stopMenuMusic()
    {
        song.Stop();
    }
}
