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

    //animation
    [SerializeField] private GameObject[] effects = new GameObject[GAYAGEUM_SCALES_COUNT];    

    private Vector3[] print_locations = new Vector3[GAYAGEUM_SCALES_COUNT];
    private Dictionary<int, int> gayageum_scale_dictionary = new Dictionary<int, int>(); //���߱� - �ұ� ���� ����
    public Queue<Transform>[] unity_editor_current_scales_gayageum = new Queue<Transform>[GAYAGEUM_SCALES_COUNT]; // ���� ���ӿ��� �Ǻ��� �����ϴ� ������ �ǽð� ��ǥ
    public Queue<GameObject>[] unity_editor_current_scales_gameobject_gayageum = new Queue<GameObject>[GAYAGEUM_SCALES_COUNT]; //����ȭ�� ���� ���� ����
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

        //Key - value : (�ұ��� ��� �� - ���߱��� ��� ° ��)
        //enum���� �ٿ� �´� �ڷᱸ�� ��ȣ�� ��������
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
        int sogeum_scale_index = 0;
        int gayageum_container_index = 0;
        float beat_value = 0;

        for(int i=0; i<selected_list.Count; i++)
        {
            sogeum_scale_index = selected_list[i].Item1;
            
            //���� ����
            if (sogeum_scale_index == -1)
            {                         
                WorksAfterGameEnd();
                break;
            }                 
            //�Ʒ� 3�� �б⿡�� �ұ��� ���� ������.
            else if(sogeum_scale_index == 0)
            {
                //�ұ� �� ���� X
            }
            else if(1<= sogeum_scale_index && sogeum_scale_index <= MusicDataPjw.SCALE_CONTROL_VALUE)
            {
                sogeum_scale_index -= 1;
            }
            else
            {
                sogeum_scale_index -= MusicDataPjw.SCALE_CONTROL_VALUE;
            }
            gayageum_container_index = gayageum_scale_dictionary[sogeum_scale_index]; //0�� ~ 11�������� '���߱� ��-1'�� ����      
            beat_value = selected_list[i].Item2;

            if (gayageum_container_index == -1) //�ұ����� ������ �׳� �н�
            {
                //Debug.Log("�ұ��� , ���߱� �ڷᱸ�� �ε��� " + sogeum_scale_index + "  " + gayageum_container_index);
                yield return new WaitForSeconds(beat_value);
                continue;
            }

            //Debug.Log("�ұ��� , ���߱� �ڷᱸ�� �ε��� " + sogeum_scale_index + "  " + gayageum_container_index);
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

    //����Ƽ �������� UI�� �����ϴ� ��ǥ���� �ڷᱸ���� �����ϴ� �Լ�
    private void FillUnityEditorCurrentScales(int gayageum_container_index, Transform note_prefab_transform , GameObject note_prefab_gameobject)
    {
        //Debug.Log("�ڷᱸ��" + gayageum_container_index + "�� ������ ����");
        unity_editor_current_scales_gayageum[gayageum_container_index].Enqueue(note_prefab_transform);
        unity_editor_current_scales_gameobject_gayageum[gayageum_container_index].Enqueue(note_prefab_gameobject);
    }

    /*
     * 1. Index�� ������� �ұݰ� �ش� �Ǳ��� ġ������ ���� ���� �ڷᱸ�� : A-(int)GAYAGEUM_SCALE_NUMBER.ONE , S-(int)GAYAGEUM_SCALE_NUMBER.TWO , ...
     * 2. PlaySound(i) : i�� AudioClipsGroupPjw�� AudioSource�� ����� ���� ������ ����. �̰� �翬�� 0���� ����ǹǷ� i�� 0���� �������
     * ���� : �� �����͸� �ϳ��� ����ü�� Ŭ������ �����ٸ� �� ������ ��.
     */

    //vr���� collider�� �����Ǵ� ���� �ٲܵ�
    //�ڷᱸ�� �����ؾ��� : �� �ݸ����� �´� ���� ��ȣ�� ���߱� ��
    //EX : Q�� ������ �� 0�� �Ҹ��� ���;��� �ٵ� ���߱��� 0���� �����ϴ� �ұ��� ���� ����. ��� ����.

    //VR : �̰� ���� ȣ������� name���� �� �츰 �ð��� ����
    public void CheckInputs(string scale)
    {
        int index = 0; //index�� ���߱� �ٹ�ȣ�� �ǹ�
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


    //TEST�� �ڵ�
    public void PlayTest()
    {
        int index = 0; //index�� ���߱� �ٹ�ȣ�� �ǹ�
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
            //�ұݿ� �ش� ���� ����
            /*index = (int)GAYAGEUM_SCALE_NUMBER.ELEVEN;
            if (unity_editor_current_scales_gayageum[index].Count != 0)
            {
                JudgeAccuracy(index);
            }*/
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            PlaySound(11);
            //�ұݿ� �ش� ���� ����
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
        else //miss�� ��ǥ�� ������ ����
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
