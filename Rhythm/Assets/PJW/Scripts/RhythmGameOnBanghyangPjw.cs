//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;


//public class RhythmGameOnBanghyangPjw : MonoBehaviour
//{
//    private static RhythmGameOnBanghyangPjw instance;
//    public static RhythmGameOnBanghyangPjw Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = GameObject.FindObjectOfType<RhythmGameOnBanghyangPjw>();
//            }
//            return instance;
//        }
//    }

//    private const int BANGHYANG_SCALES_COUNT = 16;
//    private const float SCALE_SIZE_MULTIPLY = 0.6f;

//    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
//    public int selected_music_number;
//    [SerializeField] Transform[] starting_points = new Transform[BANGHYANG_SCALES_COUNT];
//    [SerializeField] Transform[] end_points = new Transform[BANGHYANG_SCALES_COUNT];
//    [SerializeField] GameObject note = null;

//    private Vector3[] print_locations = new Vector3[BANGHYANG_SCALES_COUNT];
//    private Dictionary<int, int> banghyang_scale_dictionary = new Dictionary<int, int>();

//    public Queue<Transform>[] unity_editor_current_scales = new Queue<Transform>[BANGHYANG_SCALES_COUNT]; //using CollisionAndUpdatingQueuePjw Class
//    public Queue<GameObject>[] unity_editor_current_scales_gameobject = new Queue<GameObject>[BANGHYANG_SCALES_COUNT]; //최적화를 위해 따로 저장
//    public int perfect_count { get; private set; }
//    public int great_count { get; private set; }
//    public int miss_count { get; private set; }

//    //should using MusicDataPjw's containers when container's data are completed.
//    private void Awake()
//    {
//        Initialize();
//    }

//    private void Update()
//    {
//        CheckInputs();
//        Test_1();
//    }

//    private void Test_1()
//    {
//        if (Input.GetKeyDown(KeyCode.Q) == true)
//        {
//            ScoreManagerPjw.Instance.BecomeActivate();
//        }
//        if (Input.GetKeyDown(KeyCode.W) == true)
//        {
//            ScoreManagerPjw.Instance.BecomeDeActivate();
//        }
//        if (Input.GetKeyDown(KeyCode.E) == true)
//        {
//            ScoreManagerPjw.Instance.MeasureScore(perfect_count, great_count, miss_count);
//        }
//    }
//    //이누야샤만이라고 생각
//    private void Initialize()
//    {
//        for (int i = 0; i < BANGHYANG_SCALES_COUNT; i++)
//        {
//            print_locations[i] = gameObject.transform.GetChild(0).transform.GetChild(i).transform.position;
//        }
//        for (int i = 0; i < BANGHYANG_SCALES_COUNT; i++)
//        {
//            unity_editor_current_scales[i] = new Queue<Transform>();
//            unity_editor_current_scales_gameobject[i] = new Queue<GameObject>();
//        }
//        //왼쪽은 소금 , 오른쪽은 방향 음
//        //방향 음은 1부터 시작하므로 저장할 때는 -1된 값으로 함
//        //소금의 음이 가야금에 없는 것은 -1
//        banghyang_scale_dictionary.Add(0, 1);
//        banghyang_scale_dictionary.Add(1, -1);
//        banghyang_scale_dictionary.Add(2, (int)GAYAGEUM_SCALE_NUMBER.THREE);
//        banghyang_scale_dictionary.Add(3, (int)GAYAGEUM_SCALE_NUMBER.FOUR);
//        banghyang_scale_dictionary.Add(4, -1);
//        banghyang_scale_dictionary.Add(5, (int)GAYAGEUM_SCALE_NUMBER.FIVE);
//        banghyang_scale_dictionary.Add(6, (int)GAYAGEUM_SCALE_NUMBER.FOUR);
//        banghyang_scale_dictionary.Add(7, (int)GAYAGEUM_SCALE_NUMBER.FIVE);
//        banghyang_scale_dictionary.Add(8, -1);
//        banghyang_scale_dictionary.Add(9, (int)GAYAGEUM_SCALE_NUMBER.SIX);
//        banghyang_scale_dictionary.Add(10, (int)GAYAGEUM_SCALE_NUMBER.SEVEN);
//        banghyang_scale_dictionary.Add(11, -1);
//        banghyang_scale_dictionary.Add(12, (int)GAYAGEUM_SCALE_NUMBER.EIGHT);
//        banghyang_scale_dictionary.Add(13, (int)GAYAGEUM_SCALE_NUMBER.NINE);
//        banghyang_scale_dictionary.Add(14, -1);
//        banghyang_scale_dictionary.Add(15, -1);

//        InitializeCountValues();

//    }

//    //새로운 게임 마다 점수 들은 모두 0으로 초기화
//    private void InitializeCountValues()
//    {
//        perfect_count = 0;
//        great_count = 0;
//        miss_count = 0;
//    }
//    public void CheckLoadDataSuccess()
//    {
//        if (selected_music_number == (int)MUSIC_NUMBER.INUYASHA)
//        {
//            selected_list = MusicDataPjw.music_inuyasha;
//        }
//        else if (selected_music_number == (int)MUSIC_NUMBER.LETITGO)
//        {
//            selected_list = MusicDataPjw.music_letitgo;
//        }
//        else if (selected_music_number == (int)MUSIC_NUMBER.LETITGO)
//        {
//            selected_list = MusicDataPjw.music_tmp;
//        }
//        /*foreach(var i in selected_list)
//        {
//            Debug.Log(i.Item1 + " " + i.Item2);
//        }*/
//    }

//    public void OrderForStartingCoroutine()
//    {
//        StartCoroutine("PrintScales");
//    }

//    IEnumerator PrintScales()
//    {
//        int index = 0;
//        float beat_value = 0;

//        for (int i = 0; i < selected_list.Count; i++)
//        {
//            index = banghyang_scale_dictionary[selected_list[i].Item1];
//            beat_value = selected_list[i].Item2;
//            //Debug.Log(beat_value);

//            if (index == -1)
//            {
//                yield return new WaitForSeconds(beat_value);
//                continue;
//            }

//            GameObject note_prefab = Instantiate(note, starting_points[index].position, new Quaternion(0, 0, 0, 0)); //spawn
//            note_prefab.transform.localScale *= SCALE_SIZE_MULTIPLY;
//            note_prefab.transform.SetParent(this.transform);
//            FillUnityEditorCurrentScales(index, note_prefab.transform, note_prefab);

//            yield return new WaitForSeconds(beat_value);
//        }
//        yield return null;
//    }

//    //유니티 에디터의 UI에 존재하는 음표들을 자료구조에 저장하는 함수
//    private void FillUnityEditorCurrentScales(int index, Transform note_prefab_transform, GameObject note_prefab_gameobject)
//    {
//        unity_editor_current_scales[index].Enqueue(note_prefab_transform);
//        unity_editor_current_scales_gameobject[index].Enqueue(note_prefab_gameobject);
//    }

//    //이것도 CollisionAndUpdatingQueuePjw 처럼 만들어도 좋을듯
//    //vr에서 collider로 구현되니 많이 바꿀듯
//    private void CheckInputs() //A-(int)GAYAGEUM_SCALE_NUMBER.ONE , S-(int)GAYAGEUM_SCALE_NUMBER.TWO , ...
//    {
//        int index = 0;
//        if (Input.GetKeyDown(KeyCode.A) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.ONE;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.S) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.TWO;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.D) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.THREE;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.F) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.FOUR;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.G) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.FIVE;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.H) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.SIX;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.J) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.SEVEN;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.K) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.EIGHT;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.L) == true)
//        {
//            index = (int)GAYAGEUM_SCALE_NUMBER.NINE;
//            if (unity_editor_current_scales[index].Count != 0)
//            {
//                JudgeAccuracy(index);
//            }
//        }
//    }

//    //악보에 음 없을 때 누르는건 아래의 함수가 작동하지 않음.
//    private void JudgeAccuracy(int index)
//    {
//        float accuracy_value = end_points[index].position.x - unity_editor_current_scales[index].Peek().position.x;
//        accuracy_value = Math.Abs(accuracy_value);

//        if (accuracy_value <= (float)SCALE_ACCURACY_EASY.EASY_PERFECT)
//        {
//            perfect_count++;
//            Debug.Log(accuracy_value + " perfect");
//        }
//        else if ((float)SCALE_ACCURACY_EASY.EASY_PERFECT < accuracy_value && accuracy_value < (float)SCALE_ACCURACY_EASY.EASY_GREAT)
//        {
//            great_count++;
//            Debug.Log(accuracy_value + " Great");
//        }
//        else //miss는 음표를 없애지 않음
//        {
//            miss_count++;
//            Debug.Log(accuracy_value + " Miss");
//        }
//    }

//    private void PlaySound()
//    {

//    }
//}
