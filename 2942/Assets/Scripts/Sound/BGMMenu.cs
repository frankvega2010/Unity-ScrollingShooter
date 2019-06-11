using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMenu : MonoBehaviourSingleton<BGMMenu>
{
    //public GameObject AudioSourceObject;
    public AudioSource song;

    private void Start()
    {
        song = GetComponent<AudioSource>();
    }

    public void playMenuMusic()
    {
        song.Play();
    }

    public void stopMenuMusic()
    {
        song.Stop();
    }
}
