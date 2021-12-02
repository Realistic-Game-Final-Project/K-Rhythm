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
    private const int GAYAGEUM_SCALES_COUNT = 12; 
    private const int LEFT_PADDING = 99;
    private const float LOOP_CNT = 50f;
    private const float SCALE_SIZE_MULTIPLY = 0.6f;

    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
    public int selected_music_number;    
    [SerializeField] Transform[] starting_points = new Transform[GAYAGEUM_SCALES_COUNT];
    [SerializeField] GameObject note = null;

    private int index = 0;
    private Vector3[] print_locations = new Vector3[GAYAGEUM_SCALES_COUNT];
    private Dictionary<int, int> gayageum_scale_dictionary = new Dictionary<int, int>();

    private void Awake()
    {
        Initialize();
    }
    //should using MusicDataPjw's containers when container's data are completed.

    private void Update()
    {
        
    }
    private void Initialize()
    {
        Debug.Log(gameObject.transform.GetChild(0).name);
        //이누야샤만이라고 생각
        for(int i=0; i<GAYAGEUM_SCALES_COUNT; i++)
        {
            print_locations[i] = gameObject.transform.GetChild(0).transform.GetChild(i).transform.position;                      
        }
        //왼쪽은 소금 , 오른쪽은 가야금 음
        //가야금 음은 1부터 시작하므로 저장할 때는 -1된 값으로 함
        //소금의 음이 가야금에 없는 것은 -1
        gayageum_scale_dictionary.Add(0 , - 1);
        gayageum_scale_dictionary.Add(1, -1);
        gayageum_scale_dictionary.Add(2, 3);
        gayageum_scale_dictionary.Add(3, 4);
        gayageum_scale_dictionary.Add(4, -1);
        gayageum_scale_dictionary.Add(5, 5);
        gayageum_scale_dictionary.Add(6, 6);
        gayageum_scale_dictionary.Add(7, 7);
        gayageum_scale_dictionary.Add(8, -1);
        gayageum_scale_dictionary.Add(9, 8);
        gayageum_scale_dictionary.Add(10, 9);
        gayageum_scale_dictionary.Add(11, -1);
        gayageum_scale_dictionary.Add(12, 10);
        gayageum_scale_dictionary.Add(13, 11);
        gayageum_scale_dictionary.Add(14, -1);
        gayageum_scale_dictionary.Add(15, -1);
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
     
    }
    public void OrderForStartingCoroutine()
    {
        StartCoroutine("PrintScales");
    }
    IEnumerator PrintScales()
    {
        int tmp = 0;
        for(int i=0; i<selected_list.Count; i++)
        {
            tmp = gayageum_scale_dictionary[selected_list[i].Item1];            
            if(tmp == -1)
            {
                continue;
            }
            Debug.Log(tmp);
            GameObject note_prefab = Instantiate(note, starting_points[tmp].position ,new Quaternion(0, 0, 0, 0)); //spawn
            note_prefab.transform.localScale *= SCALE_SIZE_MULTIPLY;
            note_prefab.transform.SetParent(this.transform);          
            for (int j=0; j<LOOP_CNT; j++)
            {
                note_prefab.transform.position += new Vector3(3, 0, 0);
                yield return new WaitForSeconds(selected_list[i].Item2 / LOOP_CNT);
            }   
        }     
        yield return null;
    }

    private void CheckInputs()
    {
        if(Input.GetKeyDown(KeyCode.A) == true)
        {
            //JudgeAccuracy('a')
        }
    }

    private void JudgeAccuracy()
    {

    }

    private void PlaySound()
    {

    }
}
