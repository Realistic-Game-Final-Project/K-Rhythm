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
    private const int SOGEUM_SCALE_COUNT = 17;
    private const float DELAY_TIME = 0; //음악이 서로 다른 순서로 나와야 함
 
    [SerializeField] private AudioClip[] sogeum_scales = new AudioClip[SOGEUM_SCALE_COUNT];       
    private AudioSource[] mp3 = new AudioSource[AUDIO_SOURCE_COUNT];

    public MyTextDataArray mytext { get; private set; } //get another script 
    TextAsset textdata;
    private int music_index = 0;
    private int sound_manager_number = 0;
    private IEnumerator coroutine_obj; 

    private void Awake()
    {        
        Initialize();      
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
            else if (StaticDataPjw.is_janggu_selected == true)
            {
                NoteManager_Lee.Instance.CheckLoadDataSuccess();
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
            else if (StaticDataPjw.is_janggu_selected == true)
            {
                NoteManager_Lee.Instance.CheckLoadDataSuccess();
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
            else if (StaticDataPjw.is_janggu_selected == true)
            {
                NoteManager_Lee.Instance.CheckLoadDataSuccess();
            }
        }
        StartTwoCoroutinesAtSameTime();
    }

    //두 개의 코루틴("Auto~" , OrderFor~에서 호출하는 코루틴)은 배경음악과 , 악보에서 게임 재생이며
    //동시에 돌아야 합니다.
    private void StartTwoCoroutinesAtSameTime()
    {
        Invoke("DelayCallCoroutine", DELAY_TIME);
        
        if (StaticDataPjw.is_gayageum_selected == true)
        {
            RhythmGameOnSelectedSheetPjw.Instance.OrderForStartingCoroutine();
        }
        else if (StaticDataPjw.is_banghyang_selected == true)
        {
            RhythmGameOnBanghyangPjw.Instance.OrderForStartingCoroutine();
        }
        //TODO : 주엽이가 장구에서 만든 코루틴을 수행
        else
        {
            NoteManager_Lee.Instance.StartNoteGameCoroutine();
        }      
    }

    private void DelayCallCoroutine()
    {
        StartCoroutine("AutoPlayBackgroundmusic");
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
        const float BEAT_DELAY_DIVISION = 4f;
        coroutine_obj = AutoPlayBackgroundmusic();

        int cur_scale = mytext.music[music_index].scale;
        float cur_beat = mytext.music[music_index].beat;

        //음악종료
        if (cur_beat == -1)
        {        
            StopCoroutine(coroutine_obj);
            //yield return으로는 제어권을 넘겨주기만 하고 아래의 코드가 동작하므로 에러가 나므로 , 강제로 코루틴 종료
        }

        int cur_sound_manager_number = ReturnSoundManagerNumber(); // 1 ~ 14
        sound_manager_number++;

        //rest
        if (cur_scale == 0)
        {
            mp3[cur_sound_manager_number].clip = null;
            mp3[cur_sound_manager_number].Play();
        }
        //note
        else
        {
            //자료구조에서 index가 음수면 안되므로 조절
            if (cur_scale >= MusicDataPjw.SCALE_CONTROL_VALUE)
            {
                mp3[cur_sound_manager_number].clip = sogeum_scales[cur_scale - MusicDataPjw.SCALE_CONTROL_VALUE];
            }
            else
            {
                mp3[cur_sound_manager_number].clip = sogeum_scales[cur_scale - 1]; //소금으로도 구현이 안된 음이 있으므로 (이누야샤 2개) 그냥 -1
            }
            mp3[cur_sound_manager_number].Play();   
        }    

        yield return new WaitForSeconds(cur_beat);
        music_index++;      

        StartCoroutine(AutoPlayBackgroundmusic());
        yield return new WaitForSeconds(cur_beat / BEAT_DELAY_DIVISION);   
        StopCoroutine(coroutine_obj);
        mp3[cur_sound_manager_number].Stop();      
    }
}
