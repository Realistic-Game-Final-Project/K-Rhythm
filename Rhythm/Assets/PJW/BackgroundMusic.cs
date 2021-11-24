using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalesInfo
{
    private int num;
    private AudioClip clip;

    ScalesInfo(int num , AudioClip clip)
    {
        this.num = num;
        this.clip = clip;
    }
}

public class BackgroundMusic : MonoBehaviour
{
    public const float SCALE_DURATION = 3f; //later , const -> value
    public const int SCALES_COUNT = 3;
    public const int AUDIO_SOURCE_COUNT = 4;

    public AudioClip[] original_clips = new AudioClip[SCALES_COUNT];
    private AudioSource[] mp3 = new AudioSource[AUDIO_SOURCE_COUNT];
    private IEnumerator coroutine_obj;

    private void Awake()
    {
        Initialize();
        StartCoroutine(PlayBackgroundMusic());      
    }

    private void Initialize()
    {       
        ScalesInfo[] scales_info = new ScalesInfo[SCALES_COUNT];
        for(int i=0; i<AUDIO_SOURCE_COUNT; i++)
        {
            mp3 = gameObject.GetComponents<AudioSource>();
        }
        for(int i=0; i<SCALES_COUNT; i++)
        {
            //scales_info[i] = new ScalesInfo(i+1 , original_clips[i]);
        }        
    }

    //TODO : �ֿ��̰� ���� �����Ϳ��� �����ȣ�� �о ����� Ŭ������ ��ȯ , ���� ���� �ð� ����
    private void GetDataFromFirebaseAndConvertAudioclip()
    {
        
    }

    IEnumerator PlayBackgroundMusic()
    {
        //GetDataFromFirebaseAndConvertAudioclip()

        coroutine_obj = PlayBackgroundMusic();
        Debug.Log("coroutine Success");
        /*
         * 
         * 
         * ���⿡ ���� ���� �Ҹ� ����ϵ��� �ڵ� �ۼ�
         * - �پ��� AudioSource 
         * - �ڷ�ƾ���� ����(duration)�� �ް� �� ���� ��ŭ ���� ���� �ϵ��� ����
         */

        yield return new WaitForSeconds(SCALE_DURATION);

        StopCoroutine(coroutine_obj);
        coroutine_obj = null; //go GC?
        StartCoroutine(PlayBackgroundMusic());
    }

}
