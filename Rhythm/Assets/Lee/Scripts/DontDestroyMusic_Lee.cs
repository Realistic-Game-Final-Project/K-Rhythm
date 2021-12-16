using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic_Lee : MonoBehaviour
{
    [SerializeField]private AudioClip Idol;
    private AudioSource DDmusic;

    private void Awake()
    {
        DDmusic = GetComponent<AudioSource>();
        DDmusic.clip = Idol;
        DDmusic.Play();
        DontDestroyOnLoad(this.gameObject);
    }

}
