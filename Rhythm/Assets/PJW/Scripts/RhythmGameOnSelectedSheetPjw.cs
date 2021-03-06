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

    //animation
    [SerializeField] private GameObject[] effects = new GameObject[GAYAGEUM_SCALES_COUNT];    

    private Vector3[] print_locations = new Vector3[GAYAGEUM_SCALES_COUNT];
    private Dictionary<int, int> gayageum_scale_dictionary = new Dictionary<int, int>(); //가야금 - 소금 음계 연결
    public Queue<Transform>[] unity_editor_current_scales_gayageum = new Queue<Transform>[GAYAGEUM_SCALES_COUNT]; // 리듬 게임에서 악보에 존재하는 음계의 실시간 좌표
    public Queue<GameObject>[] unity_editor_current_scales_gameobject_gayageum = new Queue<GameObject>[GAYAGEUM_SCALES_COUNT]; //최적화를 위해 따로 저장
    public int perfect_count { get; private set; }
    public int great_count { get; private set; }
    public int miss_count { get; private set; }

    //should using MusicDataPjw's containers when container's data are completed.
    private void Awake()
    {
        Initialize();        
    }
   
    /*private void Update()
    {
        PlayTest();
    }*/
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

        //Key - value : (소금의 몇번 음 - 가야금의 몇번 째 줄)
        //enum에서 줄에 맞는 자료구조 번호를 지정했음
        gayageum_scale_dictionary.Add(0 , -1);
        gayageum_scale_dictionary.Add(1 , -1);
        gayageum_scale_dictionary.Add(2, (int)GAYAGEUM_SCALE_NUMBER.TWO);
        gayageum_scale_dictionary.Add(3, (int)GAYAGEUM_SCALE_NUMBER.THREE);
        gayageum_scale_dictionary.Add(4, -1);
        gayageum_scale_dictionary.Add(5, -1);
        gayageum_scale_dictionary.Add(6, (int)GAYAGEUM_SCALE_NUMBER.FOUR);
        gayageum_scale_dictionary.Add(7, -1);
        gayageum_scale_dictionary.Add(8, (int)GAYAGEUM_SCALE_NUMBER.FIVE);
        gayageum_scale_dictionary.Add(9, -1);
        gayageum_scale_dictionary.Add(10, (int)GAYAGEUM_SCALE_NUMBER.SIX);
        gayageum_scale_dictionary.Add(11, (int)GAYAGEUM_SCALE_NUMBER.SEVEN);
        gayageum_scale_dictionary.Add(12, (int)GAYAGEUM_SCALE_NUMBER.EIGHT);
        gayageum_scale_dictionary.Add(13, (int)GAYAGEUM_SCALE_NUMBER.NINE);
        gayageum_scale_dictionary.Add(14, (int)GAYAGEUM_SCALE_NUMBER.TEN);
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
        int sogeum_scale_index = 0;
        int gayageum_container_index = 0;
        float beat_value = 0;

        for(int i=0; i<selected_list.Count; i++)
        {
            sogeum_scale_index = selected_list[i].Item1;
            
            //음악 종료
            if (sogeum_scale_index == -1)
            {                         
                WorksAfterGameEnd();
                break;
            }                 
            //아래 3개 분기에서 소금의 값을 변경함.
            else if(sogeum_scale_index == 0)
            {
                //소금 값 변경 X
            }
            else if(1<= sogeum_scale_index && sogeum_scale_index <= MusicDataPjw.SCALE_CONTROL_VALUE)
            {
                sogeum_scale_index -= 1;
            }
            else
            {
                sogeum_scale_index -= MusicDataPjw.SCALE_CONTROL_VALUE;
            }
            gayageum_container_index = gayageum_scale_dictionary[sogeum_scale_index]; //0번 ~ 11번까지의 '가야금 줄-1'을 저장      
            beat_value = selected_list[i].Item2;

            if (gayageum_container_index == -1) //소금음이 없으면 그냥 패스
            {
                //Debug.Log("소금음 , 가야금 자료구조 인덱스 " + sogeum_scale_index + "  " + gayageum_container_index);
                yield return new WaitForSeconds(beat_value);
                continue;
            }

            //Debug.Log("소금음 , 가야금 자료구조 인덱스 " + sogeum_scale_index + "  " + gayageum_container_index);
            GameObject note_prefab = Instantiate(note, starting_points[gayageum_container_index].position ,new Quaternion(0, 0, 0, 0)); //spawn
            note_prefab.transform.localScale *= SCALE_SIZE_MULTIPLY;
            note_prefab.transform.SetParent(this.transform);            
            FillUnityEditorCurrentScales(gayageum_container_index, note_prefab.transform , note_prefab);                       
            yield return new WaitForSeconds(beat_value);
        }     
        yield return null;
    }

    private void WorksAfterGameEnd()
    {
        GameManagerPjw.Instance.is_game_ended = true;
        gameObject.SetActive(false);
        ScoreManagerPjw.Instance.MeasureScore(perfect_count, great_count, miss_count);
        GameManagerPjw.Instance.ShowScoreBoard();
    }

    //유니티 에디터의 UI에 존재하는 음표들을 자료구조에 저장하는 함수
    private void FillUnityEditorCurrentScales(int gayageum_container_index, Transform note_prefab_transform , GameObject note_prefab_gameobject)
    {
        //Debug.Log("자료구조" + gayageum_container_index + "에 데이터 저장");
        unity_editor_current_scales_gayageum[gayageum_container_index].Enqueue(note_prefab_transform);
        unity_editor_current_scales_gameobject_gayageum[gayageum_container_index].Enqueue(note_prefab_gameobject);
    }

    /*
     * 1. Index는 배경음인 소금과 해당 악기의 치는음에 대한 관계 자료구조 : A-(int)GAYAGEUM_SCALE_NUMBER.ONE , S-(int)GAYAGEUM_SCALE_NUMBER.TWO , ...
     * 2. PlaySound(i) : i는 AudioClipsGroupPjw의 AudioSource에 저장된 실제 음원의 순서. 이건 당연히 0부터 저장되므로 i는 0부터 순서대로
     * 총평 : 이 데이터를 하나의 구조체나 클래스로 묶었다면 더 좋았을 듯.
     */

    //vr에서 collider로 구현되니 많이 바꿀듯
    //자료구조 제작해야함 : 이 콜리전에 맞는 사운드 번호랑 가야금 줄
    //EX : Q를 눌렀을 때 0번 소리가 나와야함 근데 가야금의 0번에 대응하는 소금의 값이 없음. 고로 안함.

    //VR : 이건 자주 호출되지만 name으로 비교 우린 시간이 없음
    public void CheckInputs(string scale)
    {
        int index = 0; //index는 가야금 줄번호를 의미
        if(scale == "Scale")
        {
            Debug.Log("0");
            PlaySound(0);          
        }
        else if (scale == "Scale (1)")
        {
            Debug.Log("1");
            PlaySound(1);
            index = (int)GAYAGEUM_SCALE_NUMBER.TWO;       
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (2)")
        {
            Debug.Log("2");
            PlaySound(2);
            index = (int)GAYAGEUM_SCALE_NUMBER.THREE;      
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (3)")
        {
            Debug.Log("3");
            PlaySound(3);
            index = (int)GAYAGEUM_SCALE_NUMBER.FOUR;       
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (4)")
        {
            Debug.Log("4");
            PlaySound(4);
            index = (int)GAYAGEUM_SCALE_NUMBER.FIVE;       
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (5)")
        {
            Debug.Log("5");
            PlaySound(5);
            index = (int)GAYAGEUM_SCALE_NUMBER.SIX;     
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (6)")
        {
            Debug.Log("6");
            PlaySound(6);
            index = (int)GAYAGEUM_SCALE_NUMBER.SEVEN;     
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (7)")
        {
            Debug.Log("7");
            PlaySound(7);
            index = (int)GAYAGEUM_SCALE_NUMBER.EIGHT;   
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (8)")
        {
            Debug.Log("8");
            PlaySound(8);
            index = (int)GAYAGEUM_SCALE_NUMBER.NINE;      
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {                
                JudgeAccuracy(index);              
            }
        }
        else if (scale == "Scale (9)")
        {
            Debug.Log("9");
            PlaySound(9);
            index = (int)GAYAGEUM_SCALE_NUMBER.TEN;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (scale == "Scale (10)")
        {
            Debug.Log("10");
            PlaySound(10);
            /*index = (int)GAYAGEUM_SCALE_NUMBER.ELEVEN;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }*/
        }
        else if (scale == "Scale (11)")
        {
            Debug.Log("11");
            PlaySound(11);
            /*index = (int)GAYAGEUM_SCALE_NUMBER.TWELVE;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }*/
        }
    }


    //TEST용 코드
    public void PlayTest()
    {
        int index = 0; //index는 가야금 줄번호를 의미
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaySound(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            PlaySound(1);
            index = (int)GAYAGEUM_SCALE_NUMBER.TWO;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PlaySound(2);
            index = (int)GAYAGEUM_SCALE_NUMBER.THREE;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PlaySound(3);
            index = (int)GAYAGEUM_SCALE_NUMBER.FOUR;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            PlaySound(4);
            index = (int)GAYAGEUM_SCALE_NUMBER.FIVE;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySound(5);
            index = (int)GAYAGEUM_SCALE_NUMBER.SIX;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            PlaySound(6);
            index = (int)GAYAGEUM_SCALE_NUMBER.SEVEN;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlaySound(7);
            index = (int)GAYAGEUM_SCALE_NUMBER.EIGHT;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            PlaySound(8);
            index = (int)GAYAGEUM_SCALE_NUMBER.NINE;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            PlaySound(9);
            index = (int)GAYAGEUM_SCALE_NUMBER.TEN;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            PlaySound(10);
            //소금에 해당 음이 없음
            /*index = (int)GAYAGEUM_SCALE_NUMBER.ELEVEN;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }*/
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            PlaySound(11);
            //소금에 해당 음이 없음
            /*index = (int)GAYAGEUM_SCALE_NUMBER.TWELVE;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }*/
        }
    }
    private void JudgeAccuracy(int index)
    {
        PlayNoteEffect(index);
        float accuracy_value = end_points[index].position.x - unity_editor_current_scales_gayageum[index].Peek().position.x;
        accuracy_value = Math.Abs(accuracy_value);

        if (accuracy_value <= (float)SCALE_ACCURACY_EASY.EASY_PERFECT)
        {
            PlayPerfectGreatMissEffect(index, 0);
            perfect_count++;
            Debug.Log(accuracy_value + " perfect");               
        }
        else if ((float)SCALE_ACCURACY_EASY.EASY_PERFECT  < accuracy_value && accuracy_value < (float)SCALE_ACCURACY_EASY.EASY_GREAT)
        {
            PlayPerfectGreatMissEffect(index, 1);
            great_count++;
            Debug.Log(accuracy_value + " Great");           
        }      
        else //miss는 음표를 없애지 않음
        {
            PlayPerfectGreatMissEffect(index, 2);
            miss_count++;
            Debug.Log(accuracy_value + " Miss");
        }        
    }
  
    private void PlaySound(int index)
    {
        AudioClipsGroupPjw.Instance.speaker_for_playing_game.clip = AudioClipsGroupPjw.Instance.gayageum_audio_clips[index];
        AudioClipsGroupPjw.Instance.speaker_for_playing_game.Play();
    }

    private void PlayNoteEffect(int index)
    {
        effects[index].GetComponent<EffectManager_Lee>().NoteHitEffect();
    }

    private void PlayPerfectGreatMissEffect(int index, int pgm) //pgm = perfect(=0) , great(=1) , miss(=2)
    {
        effects[index].GetComponent<EffectManager_Lee>().JudgementEffect(pgm);
    }
}
