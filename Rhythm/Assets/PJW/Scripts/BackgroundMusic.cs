using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/*이 스크립트는 분할 되어야 함.
현재 음악 데이터를 json에서 읽어오고 , static에 저장하는 기능과
게임의 배경음악을 재생하는 걸 동시에 같은 스크립트에서 구현 하고 있음
하지만 배경음악 재생을 분리하는 작업이 생각보다 ERROR를 일으킬 것 같으므로 추후에
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
    //TODO : don't have to singleton
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) == true) //Space 누르면 시작하도록 일단 테스트
        {
            StartCoroutine("AutoPlayBackgroundmusic");
            RhythmGameOnSelectedSheetPjw.Instance.OrderForStartingCoroutine(); //내가 짰는데 신기하게 악보랑 배경음이랑 같은 음악을 잘 재생함.
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < AUDIO_SOURCE_COUNT; i++)
        {
            mp3 = gameObject.GetComponents<AudioSource>();
        }                
    }

    public void SelectMusicAndSaveStaticContainers()
    {
        int selected_music_index = GetMusicDataFromStatic();
        Debug.Log("음악 고름" + selected_music_index);
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
            RhythmGameOnSelectedSheetPjw.Instance.CheckLoadDataSuccess();
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
            RhythmGameOnSelectedSheetPjw.Instance.CheckLoadDataSuccess();           
        }
        else //CANNON
        {

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
        RhythmGameOnSelectedSheetPjw.Instance.selected_music_number = num;
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

        int cur_sound_manager_number = ReturnSoundManagerNumber(); // 1 ~ 14
        sound_manager_number++; 
        coroutine_obj = AutoPlayBackgroundmusic();
       
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
        music_index++; //if game end -> let music_index 0 (this makes ERROR)
        StartCoroutine(AutoPlayBackgroundmusic());
        yield return new WaitForSeconds(mytext.music[music_index].beat / BEAT_DELAY_DIVISION);
        StopCoroutine(coroutine_obj);
        mp3[cur_sound_manager_number].Stop();        
        //yield return new WaitForSeconds(mytext.music[music_index].beat / 10f);            
    }
}
