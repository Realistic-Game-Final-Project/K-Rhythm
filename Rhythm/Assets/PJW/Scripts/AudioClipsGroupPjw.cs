using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipsGroupPjw : MonoBehaviour
{
    private static AudioClipsGroupPjw instance;
    public static AudioClipsGroupPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AudioClipsGroupPjw>();
            }
            return instance;
        }
    }

    private const int GAYAGEUM_CLIPS_COUNT = 12;
    private const int BANGHYANG_CLIPS_COUNT = 16;
    private const int JANGGU_CLIPS_COUNT = 3;

    //어디서든 사용할 수 있게 public
    public AudioClip[] gayageum_audio_clips = new AudioClip[GAYAGEUM_CLIPS_COUNT];
    public AudioClip[] banghyang_audio_clips = new AudioClip[BANGHYANG_CLIPS_COUNT];
    public AudioClip[] janggu_audio_clips = new AudioClip[JANGGU_CLIPS_COUNT];
    public AudioSource speaker_for_playing_game;

    private void Awake()
    {
        speaker_for_playing_game = gameObject.GetComponent<AudioSource>();
    }
}
