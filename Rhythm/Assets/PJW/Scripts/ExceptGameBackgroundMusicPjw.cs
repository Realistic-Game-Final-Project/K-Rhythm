using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptGameBackgroundMusicPjw : MonoBehaviour
{
    private AudioSource always_playing_background_music_except_game_playing;
    [SerializeField] private AudioClip hwanghon;

    private void Awake()
    {
        always_playing_background_music_except_game_playing = gameObject.GetComponent<AudioSource>();   
        always_playing_background_music_except_game_playing.clip = hwanghon;
        always_playing_background_music_except_game_playing.Play();
        DontDestroyOnLoad(gameObject);
    }
}
