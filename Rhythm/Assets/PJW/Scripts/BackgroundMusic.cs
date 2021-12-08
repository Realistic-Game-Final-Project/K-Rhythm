using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/*
 * RhythmGameOnBanghyangPjw 와 RhythmGameOnSelectedSheetPjw는 비슷한 작업을 함에도 불구하고
 * 내 판단하에 스크립트를 분할 시켰다. 이 둘은 사용자가 선택한 악기에 따라 같은 함수를 호출하지만
 * 연결된 객체들이 모두 달라서 구분했다.
 * 그러나 해당 스크립트들을 모두 싱글턴으로 구현했고 , 이를 BackgroundMusic(현재 스크립트)에서 
 * 호출하는데 , 결국 if-else의 분기가 발생한다.
 * 이런걸 어떻게 구현할지는 공부가 더 필요할 듯 하다.
 */

[System.Serializable]
public class BackgroundMusicData
{
    public int scale;
    public float beat;
}

[System.Serializable]
public class MyTextDataArray
{
    public BackgroundMusicData[] music; // Must object name == json key
}

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    public static BackgroundMusic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BackgroundMusic>();
            }
            return instance;
        }
    }

    public const int AUDIO_SOURCE_COUNT = 14;
    TextAsset textdata;
    public MyTextDataArray mytext { get; private set; } //get another script  
    public new AudioClip[] audio;
    private AudioSource[] mp3 = new AudioSource[AUDIO_SOURCE_COUNT];    
    private int music_index = 0;
    private int sound_manager_number = 0;
    private IEnumerator coroutine_obj;
    
    private void Awake()
    {
        Initialize();      
    }

    //BackgroundMusic이 시작되면 악보의 게임도 시작됩니다.
    //일단은 space 눌러야 겜 시작함.
    //음악 안고르고(SelectMusicPjwScript를 안거치고) space 누르면 에러남. 어차피 space 누르면 나오도록 구현 안할꺼임
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true) 
        {
            StartCoroutine("AutoPlayBackgroundmusic");

            //두 개의 코루틴("Auto~" , OrderFor~에서 호출하는 코루틴)은 동시에 돌아야 합니다.
            if (StaticDataPjw.is_gayageum_selected == true)
            {
                RhythmGameOnSelectedSheetPjw.Instance.OrderForStartingCoroutine();
            }
            else if (StaticDataPjw.is_banghyang_selected == true)
            {
                RhythmGameOnBanghyangPjw.Instance.OrderForStartingCoroutine();
            }   
            //TODO : 장구에서 만든 코루틴을 수행
            else
            {

            }
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < AUDIO_SOURCE_COUNT; i++)
        {
            mp3 = gameObject.GetComponents<AudioSource>();
        }                
    }

    //함수 분할 했어야...
    public void SelectMusicAndSaveStaticContainers()
    {
        int selected_music_index = GetMusicDataFromStatic();
        Debug.Log("음악 번호 1~3 :  " + selected_music_index);
        if(selected_music_index == -1)
        {
            Debug.Log("MUSIC SELECT ERROR !");
            return;
        }
        if (selected_music_index == (int)MUSIC_NUMBER.INUYASHA)
        {
            textdata = Resources.Load("Inuyasha") as TextAsset;
            mytext = JsonUtility.FromJson<MyTextDataArray>(textdata.ToString());
            SetSelectedMusicNumber((int)MUSIC_NUMBER.INUYASHA);           
     
            for (int i = 0; i < mytext.music.Length; i++)
            {
                MusicDataPjw.music_inuyasha.Add(new Tuple<int, float>(mytext.music[i].scale, mytext.music[i].beat));
            }
            if (StaticDataPjw.is_gayageum_selected == true)
            {
                RhythmGameOnSelectedSheetPjw.Instance.CheckLoadDataSuccess();
            }
            else if (StaticDataPjw.is_banghyang_selected == true)
            {
                RhythmGameOnBanghyangPjw.Instance.CheckLoadDataSuccess();
            }         
        }
        else if (selected_music_index == (int)MUSIC_NUMBER.LETITGO)
        {
            textdata = Resources.Load("LetItGo") as TextAsset;
            mytext = JsonUtility.FromJson<MyTextDataArray>(textdata.ToString());        
            SetSelectedMusicNumber((int)MUSIC_NUMBER.LETITGO);  
            for (int i = 0; i < mytext.music.Length; i++)
            {
                MusicDataPjw.music_letitgo.Add(new Tuple<int, float>(mytext.music[i].scale, mytext.music[i].beat));
            }
            if (StaticDataPjw.is_gayageum_selected == true)
            {
                RhythmGameOnSelectedSheetPjw.Instance.CheckLoadDataSuccess();
            }
            else if (StaticDataPjw.is_banghyang_selected == true)
            {
                RhythmGameOnBanghyangPjw.Instance.CheckLoadDataSuccess();
            }
        }
        else if (selected_music_index == (int)MUSIC_NUMBER.CANNON) 
        {
            textdata = Resources.Load("Cannon") as TextAsset;
            mytext = JsonUtility.FromJson<MyTextDataArray>(textdata.ToString());
            SetSelectedMusicNumber((int)MUSIC_NUMBER.CANNON);
            for (int i = 0; i < mytext.music.Length; i++)
            {
                MusicDataPjw.music_cannon.Add(new Tuple<int, float>(mytext.music[i].scale, mytext.music[i].beat));
            }
            if (StaticDataPjw.is_gayageum_selected == true)
            {
                RhythmGameOnSelectedSheetPjw.Instance.CheckLoadDataSuccess();
            }
            else if (StaticDataPjw.is_banghyang_selected == true)
            {
                RhythmGameOnBanghyangPjw.Instance.CheckLoadDataSuccess();
            }
        }
    }

    private int GetMusicDataFromStatic()
    {
        if (StaticDataPjw.is_inuyasha_selected == true)
        {
            return (int)MUSIC_NUMBER.INUYASHA;
        }
        else if (StaticDataPjw.is_letitgo_selected == true)
        {
            return (int)MUSIC_NUMBER.LETITGO;
        }
        else if (StaticDataPjw.is_cannon_selected == true)
        {
            return (int)MUSIC_NUMBER.CANNON;
        }
        else
        {
            return -1;
        }
    }
    private void SetSelectedMusicNumber(int num)
    {
        if (StaticDataPjw.is_gayageum_selected == true)
        {
            RhythmGameOnSelectedSheetPjw.Instance.selected_music_number = num;
        }
        else if (StaticDataPjw.is_banghyang_selected == true)
        {
            RhythmGameOnBanghyangPjw.Instance.selected_music_number = num;
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
         *TODO : Music end -> Coroutine end
         */
        const float BEAT_DELAY_DIVISION = 4f;
        coroutine_obj = AutoPlayBackgroundmusic();
        int cur_sound_manager_number = ReturnSoundManagerNumber(); // 1 ~ 14
        sound_manager_number++; 
               
        //rest
        if (mytext.music[music_index].scale == 0)
        {
            mp3[cur_sound_manager_number].clip = null;
            mp3[cur_sound_manager_number].Play();
        }
        //note
        else
        {
            mp3[cur_sound_manager_number].clip = audio[mytext.music[music_index].scale - 1];
            mp3[cur_sound_manager_number].Play();
        }        
        yield return new WaitForSeconds(mytext.music[music_index].beat);
        music_index++; 
        StartCoroutine(AutoPlayBackgroundmusic());
        yield return new WaitForSeconds(mytext.music[music_index].beat / BEAT_DELAY_DIVISION);
        StopCoroutine(coroutine_obj);
        mp3[cur_sound_manager_number].Stop();      
    }
}
