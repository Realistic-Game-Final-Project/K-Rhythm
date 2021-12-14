using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RhythmGameOnBanghyangPjw : MonoBehaviour
{
    private static RhythmGameOnBanghyangPjw instance;
    public static RhythmGameOnBanghyangPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<RhythmGameOnBanghyangPjw>();
            }
            return instance;
        }
    }

    private const int BANGHYANG_SCALES_COUNT = 16;
    private const float SCALE_SIZE_MULTIPLY = 0.6f;

    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
    public int selected_music_number;
    [SerializeField] Transform[] starting_points = new Transform[BANGHYANG_SCALES_COUNT];
    [SerializeField] Transform[] end_points = new Transform[BANGHYANG_SCALES_COUNT];
    [SerializeField] GameObject note = null;

    private Vector3[] print_locations = new Vector3[BANGHYANG_SCALES_COUNT];
    private Dictionary<int, int> banghyang_scale_dictionary = new Dictionary<int, int>();
    public Queue<Transform>[] unity_editor_current_scales_banghyang = new Queue<Transform>[BANGHYANG_SCALES_COUNT]; //using CollisionAndUpdatingQueuePjw Class
    public Queue<GameObject>[] unity_editor_current_scales_gameobject_banghyang = new Queue<GameObject>[BANGHYANG_SCALES_COUNT]; //최적화를 위해 따로 저장   

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
    }

    //이누야샤만이라고 생각
    private void Initialize()
    {
        for (int i = 0; i < BANGHYANG_SCALES_COUNT; i++)
        {
            print_locations[i] = gameObject.transform.GetChild(0).transform.GetChild(i).transform.position;
        }
        for (int i = 0; i < BANGHYANG_SCALES_COUNT; i++)
        {
            unity_editor_current_scales_banghyang[i] = new Queue<Transform>();
            unity_editor_current_scales_gameobject_banghyang[i] = new Queue<GameObject>();
        }
        //왼쪽은 소금 , 오른쪽은 방향 음
        //방향 음은 1부터 시작하므로 저장할 때는 -1된 값으로 함
        //소금의 음이 방향에 없는 것은 -1
        banghyang_scale_dictionary.Add(0, -1);
        banghyang_scale_dictionary.Add(1, (int)BANGHYANG_SCALE_NUMBER.ONE);
        banghyang_scale_dictionary.Add(2, (int)BANGHYANG_SCALE_NUMBER.THREE);
        banghyang_scale_dictionary.Add(3, (int)BANGHYANG_SCALE_NUMBER.FIVE);
        banghyang_scale_dictionary.Add(4, (int)BANGHYANG_SCALE_NUMBER.SIX);
        banghyang_scale_dictionary.Add(5, (int)BANGHYANG_SCALE_NUMBER.EIGHT);
        banghyang_scale_dictionary.Add(6, (int)BANGHYANG_SCALE_NUMBER.TEN);
        banghyang_scale_dictionary.Add(7, (int)BANGHYANG_SCALE_NUMBER.TWELVE);
        banghyang_scale_dictionary.Add(8, (int)BANGHYANG_SCALE_NUMBER.THIRTEEN);
        banghyang_scale_dictionary.Add(9, (int)BANGHYANG_SCALE_NUMBER.FIFTEEN);
        banghyang_scale_dictionary.Add(10, -1);
        banghyang_scale_dictionary.Add(11, -1);
        banghyang_scale_dictionary.Add(12, -1);
        banghyang_scale_dictionary.Add(13, -1);
        banghyang_scale_dictionary.Add(14, -1);
        banghyang_scale_dictionary.Add(15, -1);

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

        for (int i = 0; i < selected_list.Count; i++)
        {
            //Test
            /*if(i==5)
            {
                Debug.Log("방향 종료");
                WorksAfterGameEnd();
                break;
            }*/


            //음악 종료
            if(selected_list[i].Item1 == -1)
            {            
                WorksAfterGameEnd();                
                break;
            }


            index = banghyang_scale_dictionary[selected_list[i].Item1]; //선택된 음악에 저장된 scale을 index에 저장.
            beat_value = selected_list[i].Item2;
           
            if (index == -1)
            {
                yield return new WaitForSeconds(beat_value);
                continue;
            }

            GameObject note_prefab = Instantiate(note, starting_points[index].position, new Quaternion(0, 0, 0, 0)); //spawn        
            note_prefab.transform.localScale *= SCALE_SIZE_MULTIPLY;
            note_prefab.transform.SetParent(this.transform);        
            FillUnityEditorCurrentScales(index, note_prefab.transform, note_prefab);

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
    private void FillUnityEditorCurrentScales(int index, Transform note_prefab_transform, GameObject note_prefab_gameobject)
    {
        unity_editor_current_scales_banghyang[index].Enqueue(note_prefab_transform);
        unity_editor_current_scales_gameobject_banghyang[index].Enqueue(note_prefab_gameobject);
    }


    //수정하면 짧아질 듯
    //vr에서 collider로 구현되니 많이 바꿀듯
    private void CheckInputs() //A-(int)GAYAGEUM_SCALE_NUMBER.ONE , S-(int)GAYAGEUM_SCALE_NUMBER.TWO , ...
    {
        int index = 0;
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            PlaySound(0);
            index = (int)BANGHYANG_SCALE_NUMBER.ONE;          
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            PlaySound(1);
            index = (int)BANGHYANG_SCALE_NUMBER.THREE;          
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            PlaySound(2);
            index = (int)BANGHYANG_SCALE_NUMBER.FIVE;       
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            PlaySound(3);
            index = (int)BANGHYANG_SCALE_NUMBER.SIX;          
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.T) == true)
        {
            PlaySound(4);
            index = (int)BANGHYANG_SCALE_NUMBER.EIGHT;          
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.Y) == true)
        {
            PlaySound(5);
            index = (int)BANGHYANG_SCALE_NUMBER.TEN;        
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            PlaySound(6);
            index = (int)BANGHYANG_SCALE_NUMBER.TWELVE;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            PlaySound(7);
            index = (int)BANGHYANG_SCALE_NUMBER.TWELVE;     
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            PlaySound(8);
            index = (int)BANGHYANG_SCALE_NUMBER.THIRTEEN;        
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.F) == true)
        {
            PlaySound(9);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;        
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.G) == true)
        {
            PlaySound(10);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.H) == true)
        {
            PlaySound(11);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) == true)
        {
            PlaySound(12);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.X) == true)
        {
            PlaySound(13);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.C) == true)
        {
            PlaySound(14);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.V) == true)
        {
            PlaySound(15);
            index = (int)BANGHYANG_SCALE_NUMBER.FIFTEEN;
            if (unity_editor_current_scales_banghyang[index].Count != 0)
            {
                JudgeAccuracy(index);
            }
        }
       
    }

    //악보에 음 없을 때 누르는건 아래의 함수가 작동하지 않음.
    private void JudgeAccuracy(int index)
    {
        float accuracy_value = end_points[index].position.y - unity_editor_current_scales_banghyang[index].Peek().position.y;
        accuracy_value = Math.Abs(accuracy_value);

        if (accuracy_value <= (float)SCALE_ACCURACY_EASY.EASY_PERFECT)
        {
            perfect_count++;            
            Debug.Log(accuracy_value + " perfect");
        }
        else if ((float)SCALE_ACCURACY_EASY.EASY_PERFECT < accuracy_value && accuracy_value < (float)SCALE_ACCURACY_EASY.EASY_GREAT)
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
        AudioClipsGroupPjw.Instance.speaker_for_playing_game.clip = AudioClipsGroupPjw.Instance.banghyang_audio_clips[index];
        AudioClipsGroupPjw.Instance.speaker_for_playing_game.Play();
    }

    /*private void PlayerAnimate(int index)
    {
        end_points[index].GetComponentInChildren<EffectManager_Lee>().JudgementEffect(0);
        end_points[index].GetComponentInChildren<EffectManager_Lee>().NoteHitEffect();
    }*/
}
