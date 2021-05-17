using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource musicsource;
    public AudioClip menumusic;
    public AudioClip gamemusic;
    private void Start()
    {
        musicsource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        musicsource.clip = menumusic;
    }
    public void GameStarted()
    {
        musicsource.Stop();
        musicsource.clip = gamemusic;
        musicsource.Play();
    }
    public void GameEnded()
    {
        musicsource.Stop();
        musicsource.clip = menumusic;
        musicsource.Play();
    }
}
