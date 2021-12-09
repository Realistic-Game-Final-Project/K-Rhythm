using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayJangGu : MonoBehaviour
{
    public const int SCALES_COUNT = 2;

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
        hitting_point_parent = GameObject.FindGameObjectWithTag("HittingPointJanggu").transform;
        Debug.Log(hitting_point_parent.name);
        mp3 = gameObject.GetComponent<AudioSource>();
        for (int i = 0; i < SCALES_COUNT; i++) //using index , never change
        {
            hitting_points[i] = hitting_point_parent.GetChild(i).gameObject;
            music_dictionary.Add(hitting_points[i], clips[i]);
        }
    }

    public void PlayInstrument(GameObject scale_object)
    {
        /*foreach(var a in music_dictionary)
        {
            Debug.Log(a.Key + " " + a.Value);
        }*/

        mp3.clip = music_dictionary[scale_object];
        mp3.Play(); //don't have to set end-time.
    }
}
