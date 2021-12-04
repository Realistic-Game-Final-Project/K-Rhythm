using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

//모든 스크립트에서 사용할 데이터 저장
enum MUSIC_NUMBER
{
    INUYASHA = 1,
    LETITGO = 2,
    TMP = 3
}

//Every-Script Can Use Music Data 
public class MusicDataPjw : MonoBehaviour
{
    //now just using music_inuyasha
    public static List<Tuple<int, float>> music_inuyasha = new List<Tuple<int, float>>();
    public static List<Tuple<int, float>> music_letitgo = new List<Tuple<int, float>>();
    public static List<Tuple<int, float>> music_tmp = new List<Tuple<int, float>>();    
}
