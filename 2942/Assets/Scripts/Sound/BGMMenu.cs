using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMMenu : MonoBehaviourSingleton<BGMMenu>
{
    public AudioSource song;

    private void Start()
    {
        song = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name != "GameOver")
        {
            song.Play();
        }
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
