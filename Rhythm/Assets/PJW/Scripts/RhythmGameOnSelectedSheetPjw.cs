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
    public List<Tuple<int, float>> selected_list = new List<Tuple<int, float>>();
    public int selected_music_number;
    [SerializeField]
    private GameObject circle;

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

    private const int SCALES_COUNT = 12;
    private const int LEFT_PADDING = 99;
    private const float LOOP_CNT = 50f;
    private int index = 0;
    private Vector3[] print_locations = new Vector3[SCALES_COUNT];

    private void Awake()
    {
        Initialize();
    }
    //should using MusicDataPjw's containers when container's data are completed.

    private void Initialize()
    {
        //이누야샤만이라고 생각
        for(int i=0; i<SCALES_COUNT; i++)
        {
            print_locations[i] = gameObject.transform.GetChild(i).transform.position;
            //Debug.Log(print_locations[i]);
        }        
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
        Debug.Log("크기 :" + selected_list.Count);
        //foreach (var i in selected_list)
        //{
        //    Debug.Log(i.Item1 + " " + i.Item2);
        //}
    }
    public void OrderForStartingCoroutine()
    {
        StartCoroutine("PrintScales");
    }
    IEnumerator PrintScales()
    {
        for(int i=0; i<selected_list.Count; i++)
        {
            Debug.Log(selected_list[i].Item1);
            GameObject circle_prefab = Instantiate(circle, print_locations[selected_list[i].Item1] - new Vector3(LEFT_PADDING, 0, 0), new Quaternion(0, 0, 0, 0)); //spawn
            for(int j=0; j<LOOP_CNT; j++)
            {
                circle_prefab.transform.position += new Vector3(3, 0, 0);
                yield return new WaitForSeconds(selected_list[i].Item2 / LOOP_CNT);
            }
            Destroy(circle_prefab);
        }     
        yield return null;
    }
}
