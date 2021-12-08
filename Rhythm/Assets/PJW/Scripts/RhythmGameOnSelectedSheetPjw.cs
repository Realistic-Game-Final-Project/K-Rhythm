using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//������� perfect , great , �ڷ�ƾ ����

/* ���� �̽�
3���� �Ǳ��� ���� ����� ����� �Ŷ�� �����ϰ� �ϳ��� ��ũ��Ʈ�� ����
������ ���� ����� ���� �𸥴�(���� ��ȹ�� �� ��)
�̷� ���� ��ũ��Ʈ�� 3���� ������ �ϴ°� , �ƴϸ� ũ�� ����� ����� �ؾ� �ϴ°�...
*/
//�ϴ��� �̴��߻� �뷡�θ� �����ϰ� ������ �غ���. (12/01)

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
    
    private const int GAYAGEUM_SCALES_COUNT = 12; //������ ����ϴ� ���� 9������ �Ǻ��� 12���Դϴ�.   
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
    public Queue<GameObject>[] unity_editor_current_scales_gameobject_gayageum = new Queue<GameObject>[GAYAGEUM_SCALES_COUNT]; //����ȭ�� ���� ���� ����
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

        //�ұ� - ���߱�
        //�ұ��� 0�� �� , �ұ��� 2�� �� , ...
        //���߱��� �� ��ȣ�� ��Ÿ���µ� , ���߱��� �� ��ȣ�� 0���� ����       
        //�� �ڵ�� 100% ����
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

    //���ο� ���� ���� ���� ���� ��� 0���� �ʱ�ȭ
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

    //����Ƽ �������� UI�� �����ϴ� ��ǥ���� �ڷᱸ���� �����ϴ� �Լ�
    private void FillUnityEditorCurrentScales(int index , Transform note_prefab_transform , GameObject note_prefab_gameobject)
    {
        unity_editor_current_scales_gayageum[index].Enqueue(note_prefab_transform);
        unity_editor_current_scales_gameobject_gayageum[index].Enqueue(note_prefab_gameobject);
    }

    /*
     * 1. Index�� ������� �ұݰ� �ش� �Ǳ��� ġ������ ���� ���� �ڷᱸ�� : A-(int)GAYAGEUM_SCALE_NUMBER.ONE , S-(int)GAYAGEUM_SCALE_NUMBER.TWO , ...
     * 2. PlaySound(i) : i�� AudioClipsGroupPjw�� AudioSource�� ����� ���� ������ ����. �̰� �翬�� 0���� ����ǹǷ� i�� 0���� �������
     * ���� : �� �����͸� �ϳ��� ����ü�� Ŭ������ �����ٸ� �� ������ ��.
     */

    //vr���� collider�� �����Ǵ� ���� �ٲܵ�
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

    //�Ǻ��� �� ���� �� �����°� �Ʒ��� �Լ��� �۵����� ����.
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
        else //miss�� ��ǥ�� ������ ����
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
