using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/*
 * RhythmGameOnBanghyangPjw �� RhythmGameOnSelectedSheetPjw�� ����� �۾��� �Կ��� �ұ��ϰ�
 * �� �Ǵ��Ͽ� ��ũ��Ʈ�� ���� ���״�. �� ���� ����ڰ� ������ �Ǳ⿡ ���� ���� �Լ��� ȣ��������
 * ����� ��ü���� ��� �޶� �����ߴ�.
 * �׷��� �ش� ��ũ��Ʈ���� ��� �̱������� �����߰� , �̸� BackgroundMusic(���� ��ũ��Ʈ)���� 
 * ȣ���ϴµ� , �ᱹ if-else�� �бⰡ �߻��Ѵ�.
 * �̷��� ��� ���������� ���ΰ� �� �ʿ��� �� �ϴ�.
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
    private const float DELAY_TIME = 0; //������ ���� �ٸ� ������ ���;� ��
 
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

    //�Լ� ���� �߾��...
    public void SelectMusicAndSaveStaticContainers()
    {
        int selected_music_index = GetMusicDataFromStatic();
        Debug.Log("���� ��ȣ 1~3 :  " + selected_music_index);
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

    //�� ���� �ڷ�ƾ("Auto~" , OrderFor~���� ȣ���ϴ� �ڷ�ƾ)�� ������ǰ� , �Ǻ����� ���� ����̸�
    //���ÿ� ���ƾ� �մϴ�.
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
        //TODO : �ֿ��̰� �屸���� ���� �ڷ�ƾ�� ����
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

        //��������
        if (cur_beat == -1)
        {        
            StopCoroutine(coroutine_obj);
            //yield return���δ� ������� �Ѱ��ֱ⸸ �ϰ� �Ʒ��� �ڵ尡 �����ϹǷ� ������ ���Ƿ� , ������ �ڷ�ƾ ����
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
            //�ڷᱸ������ index�� ������ �ȵǹǷ� ����
            if (cur_scale >= MusicDataPjw.SCALE_CONTROL_VALUE)
            {
                mp3[cur_sound_manager_number].clip = sogeum_scales[cur_scale - MusicDataPjw.SCALE_CONTROL_VALUE];
            }
            else
            {
                mp3[cur_sound_manager_number].clip = sogeum_scales[cur_scale - 1]; //�ұ����ε� ������ �ȵ� ���� �����Ƿ� (�̴��߻� 2��) �׳� -1
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
