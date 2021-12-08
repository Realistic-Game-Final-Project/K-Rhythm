using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//리듬게임 perfect , great , 코루틴 구현

/* 개발 이슈
3가지 악기의 동작 방식이 비슷할 거라고 생각하고 하나의 스크립트에 구현
하지만 완전 비슷할 지는 모른다(아직 기획이 덜 됨)
이럴 때는 스크립트를 3개로 나눠야 하는가 , 아니면 크게 만들고 상속을 해야 하는가...
*/
//일단은 이누야샤 노래로만 생각하고 구현을 해보자. (12/01)

public class RhythmGameOnSelectedSheetPjw : MonoBehaviour
{
    private static RhythmGameOnSelectedSheetPjw instance;
    public static RhythmGameOnSelectedSheetPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<RhythmGameOnSelectedSheetPjw>();
            }
            return instance;
        }
    }   
    
    private const int GAYAGEUM_SCALES_COUNT = 12; //실제로 사용하는 음은 9개지만 악보가 12줄입니다.   
    private const float SCALE_SIZE_MULTIPLY = 0.6f;

    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
    public int selected_music_number;    
    [SerializeField] Transform[] starting_points = new Transform[GAYAGEUM_SCALES_COUNT];
    [SerializeField] Transform[] end_points = new Transform[GAYAGEUM_SCALES_COUNT];
    [SerializeField] GameObject note = null;
    [SerializeField] private AudioSource mp3;
        
    private Vector3[] print_locations = new Vector3[GAYAGEUM_SCALES_COUNT];
    private Dictionary<int, int> gayageum_scale_dictionary = new Dictionary<int, int>();
    public Queue<Transform>[] unity_editor_current_scales_gayageum = new Queue<Transform>[GAYAGEUM_SCALES_COUNT]; //using CollisionAndUpdatingQueuePjw Class
    public Queue<GameObject>[] unity_editor_current_scales_gameobject_gayageum = new Queue<GameObject>[GAYAGEUM_SCALES_COUNT]; //최적화를 위해 따로 저장
    public int perfect_count { get; private set; }
    public int great_count { get; private set; }
    public int miss_count { get; private set; }

    //should using MusicDataPjw's containers when container's data are completed.
    private void Awake()
    {
        Initialize();        
    }
   
    private void Update()
    {
        CheckInputs();
        Test_1();        
    }

    private void Test_1()
    {
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            ScoreManagerPjw.Instance.BecomeActivate();
        }
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            ScoreManagerPjw.Instance.BecomeDeActivate();
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            ScoreManagerPjw.Instance.MeasureScore(perfect_count , great_count , miss_count);
        }
    }  
    private void Initialize()
    {        
        for(int i=0; i<GAYAGEUM_SCALES_COUNT; i++)
        {
            print_locations[i] = gameObject.transform.GetChild(0).transform.GetChild(i).transform.position;                      
        }
        for (int i = 0; i < GAYAGEUM_SCALES_COUNT; i++)
        {
            unity_editor_current_scales_gayageum[i] = new Queue<Transform>();
            unity_editor_current_scales_gameobject_gayageum[i] = new Queue<GameObject>();
        }

        //소금 - 가야금
        //소금의 0은 시 , 소금의 2는 레 , ...
        //가야금의 줄 번호를 나타내는데 , 가야금의 줄 번호는 0부터 시작       
        //이 코드는 100% 맞음
        gayageum_scale_dictionary.Add(0 , -1);
        gayageum_scale_dictionary.Add(1, -1);
        gayageum_scale_dictionary.Add(2, (int)GAYAGEUM_SCALE_NUMBER.FOUR);
        gayageum_scale_dictionary.Add(3, (int)GAYAGEUM_SCALE_NUMBER.FIVE);
        gayageum_scale_dictionary.Add(4, -1);
        gayageum_scale_dictionary.Add(5, (int)GAYAGEUM_SCALE_NUMBER.SIX);
        gayageum_scale_dictionary.Add(6, (int)GAYAGEUM_SCALE_NUMBER.SEVEN);
        gayageum_scale_dictionary.Add(7, (int)GAYAGEUM_SCALE_NUMBER.EIGHT);
        gayageum_scale_dictionary.Add(8, -1);
        gayageum_scale_dictionary.Add(9, (int)GAYAGEUM_SCALE_NUMBER.NINE);
        gayageum_scale_dictionary.Add(10, (int)GAYAGEUM_SCALE_NUMBER.TEN);
        gayageum_scale_dictionary.Add(11, -1);
        gayageum_scale_dictionary.Add(12, (int)GAYAGEUM_SCALE_NUMBER.ELEVEN);
        gayageum_scale_dictionary.Add(13, (int)GAYAGEUM_SCALE_NUMBER.TWELVE);
        gayageum_scale_dictionary.Add(14, -1);
        gayageum_scale_dictionary.Add(15, -1);

        InitializeCountValues();        
    }

    //새로운 게임 마다 점수 들은 모두 0으로 초기화
    private void InitializeCountValues()
    {
        perfect_count = 0;
        great_count = 0;
        miss_count = 0;
    }
    public void CheckLoadDataSuccess()
    {
        if (selected_music_number == (int)MUSIC_NUMBER.INUYASHA)
        {
            selected_list = MusicDataPjw.music_inuyasha;
        }
        else if (selected_music_number == (int)MUSIC_NUMBER.LETITGO)
        {
            selected_list = MusicDataPjw.music_letitgo;
        }
        else if (selected_music_number == (int)MUSIC_NUMBER.CANNON)
        {
            selected_list = MusicDataPjw.music_cannon;
        }     
    }

    public void OrderForStartingCoroutine()
    {
        StartCoroutine("PrintScales");
    }

    IEnumerator PrintScales()
    {
        int index = 0;
        float beat_value = 0;

        for(int i=0; i<selected_list.Count; i++)
        {
            index = gayageum_scale_dictionary[selected_list[i].Item1];
            beat_value = selected_list[i].Item2;        
            
            if (index == -1)
            {
                yield return new WaitForSeconds(beat_value);
                continue;
            }
          
            GameObject note_prefab = Instantiate(note, starting_points[index].position ,new Quaternion(0, 0, 0, 0)); //spawn
            note_prefab.transform.localScale *= SCALE_SIZE_MULTIPLY;
            note_prefab.transform.SetParent(this.transform);            
            FillUnityEditorCurrentScales(index , note_prefab.transform , note_prefab);
                       
            yield return new WaitForSeconds(beat_value);
        }     
        yield return null;
    }

    //유니티 에디터의 UI에 존재하는 음표들을 자료구조에 저장하는 함수
    private void FillUnityEditorCurrentScales(int index , Transform note_prefab_transform , GameObject note_prefab_gameobject)
    {
        unity_editor_current_scales_gayageum[index].Enqueue(note_prefab_transform);
        unity_editor_current_scales_gameobject_gayageum[index].Enqueue(note_prefab_gameobject);
    }

    /*
     * 1. Index는 배경음인 소금과 해당 악기의 치는음에 대한 관계 자료구조 : A-(int)GAYAGEUM_SCALE_NUMBER.ONE , S-(int)GAYAGEUM_SCALE_NUMBER.TWO , ...
     * 2. PlaySound(i) : i는 AudioClipsGroupPjw의 AudioSource에 저장된 실제 음원의 순서. 이건 당연히 0부터 저장되므로 i는 0부터 순서대로
     * 총평 : 이 데이터를 하나의 구조체나 클래스로 묶었다면 더 좋았을 듯.
     */

    //vr에서 collider로 구현되니 많이 바꿀듯
    private void CheckInputs() 
    {
        int index = 0;
        if(Input.GetKeyDown(KeyCode.A) == true)
        {
            PlaySound(0);
            index = (int)GAYAGEUM_SCALE_NUMBER.FOUR;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            PlaySound(1);
            index = (int)GAYAGEUM_SCALE_NUMBER.FIVE;       
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            PlaySound(2);
            index = (int)GAYAGEUM_SCALE_NUMBER.SIX;      
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            PlaySound(3);
            index = (int)GAYAGEUM_SCALE_NUMBER.SEVEN;       
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.G) == true)
        {
            PlaySound(4);
            index = (int)GAYAGEUM_SCALE_NUMBER.EIGHT;       
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.H) == true)
        {
            PlaySound(5);
            index = (int)GAYAGEUM_SCALE_NUMBER.NINE;     
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.J) == true)
        {
            PlaySound(6);
            index = (int)GAYAGEUM_SCALE_NUMBER.TEN;     
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            PlaySound(7);
            index = (int)GAYAGEUM_SCALE_NUMBER.ELEVEN;   
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.L) == true)
        {
            PlaySound(8);
            index = (int)GAYAGEUM_SCALE_NUMBER.TWELVE;      
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {                
                JudgeAccuracy(index);              
            }
        }
    }

    //악보에 음 없을 때 누르는건 아래의 함수가 작동하지 않음.
    private void JudgeAccuracy(int index)
    {
        float accuracy_value = end_points[index].position.x - unity_editor_current_scales_gayageum[index].Peek().position.x;
        accuracy_value = Math.Abs(accuracy_value);

        if (accuracy_value <= (float)SCALE_ACCURACY_EASY.EASY_PERFECT)
        {
            perfect_count++;
            Debug.Log(accuracy_value + " perfect");               
        }
        else if ((float)SCALE_ACCURACY_EASY.EASY_PERFECT  < accuracy_value && accuracy_value < (float)SCALE_ACCURACY_EASY.EASY_GREAT)
        {
            great_count++;

            Debug.Log(accuracy_value + " Great");           
        }      
        else //miss는 음표를 없애지 않음
        {
            miss_count++;
            Debug.Log(accuracy_value + " Miss");
        }        
    }
  
    private void PlaySound(int index)
    {
        Debug.Log(AudioClipsGroupPjw.Instance.gayageum_audio_clips[index] + " " + index);
        AudioClipsGroupPjw.Instance.speaker_for_playing_game.clip = AudioClipsGroupPjw.Instance.gayageum_audio_clips[index];
        AudioClipsGroupPjw.Instance.speaker_for_playing_game.Play();
    }
}
