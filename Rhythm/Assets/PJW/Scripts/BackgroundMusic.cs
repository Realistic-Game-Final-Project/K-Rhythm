using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BackgroundMusicData
{
    public int scale;
    public float beat;
}
[System.Serializable]
public class MyTextDataArray
{
    public BackgroundMusicData[] music; //json 파일의 키와 객체의 이름은 반드시 같아야 합니다.
}

enum MUSIC_NUMBER
{
    INUYASHA = 1,
    LETITGO = 2,
    TMP = 3       
}
public class BackgroundMusic : MonoBehaviour
{
    public const int AUDIO_SOURCE_COUNT = 14;

    TextAsset textdata;
    private MyTextDataArray mytext;
    public new AudioClip[] audio;
    private AudioSource[] mp3 = new AudioSource[AUDIO_SOURCE_COUNT];
    private int music_index = 0;
    private int sound_manager_number = 0;
    private IEnumerator coroutine_obj;

    void Awake()
    {
        Initialize();
        StartCoroutine("AutoPlayBackgroundmusic");  
    }

    private void Initialize()
    {
        for (int i = 0; i < AUDIO_SOURCE_COUNT; i++)
        {
            mp3 = gameObject.GetComponents<AudioSource>();
        }
    }

    private void SelectMusicAndLoadData(int num)
    {
        if (num == (int)MUSIC_NUMBER.INUYASHA)
        {
            textdata = Resources.Load("Inuyasha") as TextAsset;
            mytext = JsonUtility.FromJson<MyTextDataArray>(textdata.ToString());
        }
        else if (num == (int)MUSIC_NUMBER.LETITGO)
        {
            textdata = Resources.Load("LetItGo") as TextAsset;
            mytext = JsonUtility.FromJson<MyTextDataArray>(textdata.ToString());
        }
    }
    private int ReturnSoundManagerNumber()
    {
        if(sound_manager_number == AUDIO_SOURCE_COUNT-1)
        {
            sound_manager_number = 0;
        }
        return sound_manager_number;
    }
    IEnumerator AutoPlayBackgroundmusic()
    {
        /*
         *TODO : 음악 종료 시 코루틴 종료시키는 코드
         */
        

        Debug.Log(sound_manager_number);
        const float BEAT_DELAY_DIVISION = 4f;

        int cur_sound_manager_number = ReturnSoundManagerNumber(); // 1 ~ 14
        sound_manager_number++; //값 증가

        SelectMusicAndLoadData((int)MUSIC_NUMBER.LETITGO);
        coroutine_obj = AutoPlayBackgroundmusic();
       
        //0 = 쉼표
        if (mytext.music[music_index].scale == 0)
        {
            mp3[cur_sound_manager_number].clip = null;
            mp3[cur_sound_manager_number].Play();
        }
        //나머지 = 음표
        else
        {
            mp3[cur_sound_manager_number].clip = audio[mytext.music[music_index].scale - 1];
            mp3[cur_sound_manager_number].Play();
        }        
        yield return new WaitForSeconds(mytext.music[music_index].beat);
        music_index++; // 이거 음악 끝나면 다시 0 //지금은 에러 뜸
        StartCoroutine(AutoPlayBackgroundmusic());
        yield return new WaitForSeconds(mytext.music[music_index].beat / BEAT_DELAY_DIVISION);
        StopCoroutine(coroutine_obj);
        mp3[cur_sound_manager_number].Stop();        
        //yield return new WaitForSeconds(mytext.music[music_index].beat / 10f);              
 
    }
}
