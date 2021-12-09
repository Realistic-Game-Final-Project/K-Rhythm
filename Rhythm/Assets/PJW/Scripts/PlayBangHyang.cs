using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBangHyang : MonoBehaviour
{
    public const int SCALES_COUNT = 16; //maybe 16?
    
    private Transform hitting_point_parent;
    private GameObject[] hitting_points = new GameObject[SCALES_COUNT];
    public AudioClip[] clips = new AudioClip[SCALES_COUNT];
    private Dictionary<GameObject, AudioClip> music_dictionary = new Dictionary<GameObject, AudioClip>();
    private AudioSource mp3;

    private void Awake()
    {
        Initialize();                    
    }

    private void Initialize()
    {
        hitting_point_parent = GameObject.FindGameObjectWithTag("HittingPointBanghyang").transform;
        mp3 = gameObject.GetComponent<AudioSource>();
        for (int i = 0; i < SCALES_COUNT; i++) //using index , never change
        {
            hitting_points[i] = hitting_point_parent.GetChild(i).gameObject;
            music_dictionary.Add(hitting_points[i], clips[i]);
        }
    }

    public void PlayInstrument(GameObject scale_object)
    {
        mp3.clip = music_dictionary[scale_object];
        mp3.Play(); //플레이 타임 조절 없이 잘 돌아감.
    }

}
