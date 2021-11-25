using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

public class Tmp: MonoBehaviour
{

    public TextAsset inuyasha_json;

    [System.Serializable]
    public class BackgroundMusicData
    {
        public int num;
        public int duration;
    }
    [System.Serializable]
    public class BackgroundMusicArray
    {
        public BackgroundMusicData[] background_music_data;
    }

    public BackgroundMusicArray background_music_array = new BackgroundMusicArray();
    
    public const float SCALE_DURATION = 3f; //later , const -> value
    public const int SCALES_COUNT = 16;
    public const int AUDIO_SOURCE_COUNT = 4;

    public AudioClip[] original_clips = new AudioClip[SCALES_COUNT];
    private AudioSource[] mp3 = new AudioSource[AUDIO_SOURCE_COUNT];
    private BackgroundMusicData background_music_data;
    private IEnumerator coroutine_obj;

    private void Awake()
    {
        Initialize();
        StartCoroutine(PlayBackgroundMusic());
        //GetJsonFromFile();
        background_music_array = JsonUtility.FromJson<BackgroundMusicArray>(inuyasha_json.text);
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

    //TODO : 주엽이가 만든 데이터에서 음계번호를 읽어서 오디오 클립으로 변환 , 음악 지속 시간 추출  
    void GetJsonFromFile()
    {
        string json_path = Path.Combine(Application.dataPath, "PJW", "Inuyasha.json");
        //string background_music_string = File.ReadAllText(json_path);  
      
        //background_music_data = JsonUtility.FromJson<BackgroundMusicData>(background_music_string);

        ParseJson();     
    }

    private void ParseJson()
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
         * 여기에 음계 별로 소리 재생하도록 코드 작성
         * - 다양한 AudioSource 
         * - 코루틴에서 변수(duration)를 받고 그 변수 만큼 음악 지속 하도록 구현
         */

        yield return new WaitForSeconds(SCALE_DURATION);

        StopCoroutine(coroutine_obj);
        coroutine_obj = null; //go GC?
        StartCoroutine(PlayBackgroundMusic());
    }

}
