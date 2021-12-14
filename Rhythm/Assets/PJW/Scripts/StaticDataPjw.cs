using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//가야금 줄번호 = 가야금 악보 자료구조의 index
enum GAYAGEUM_SCALE_NUMBER 
{
    ONE = 0,
    TWO = 1,
    THREE = 2,
    FOUR = 3,
    FIVE = 4,
    SIX = 5,
    SEVEN = 6,
    EIGHT = 7,
    NINE = 8,
    TEN = 9,
    ELEVEN = 10,
    TWELVE = 11
}

//방향 줄번호 = 방향 악보 자료구조의 INDEX
enum BANGHYANG_SCALE_NUMBER
{
    ONE = 0,
    TWO = 1,
    THREE = 2,
    FOUR = 3,
    FIVE = 4,
    SIX = 5,
    SEVEN = 6,
    EIGHT = 7,
    NINE = 8,
    TEN = 9,
    ELEVEN = 10,
    TWELVE = 11,
    THIRTEEN = 12,
    FOURTEEN = 13,
    FIFTEEN = 14,
    SIXTEEN = 15
}

enum SCALE_ACCURACY_EASY
{
    EASY_PERFECT = 50,
    EASY_GREAT = 100
}

enum SCALE_ACCURACY_HARD
{
    HARD_PERFECT = 30,
    HARD_GREAT = 50
}

enum MUSIC_NUMBER
{
    INUYASHA = 1,
    LETITGO = 2,
    CANNON = 3
}
public class StaticDataPjw : MonoBehaviour
{
    public static bool is_banghyang_selected, is_gayageum_selected, is_janggu_selected;
    public static bool is_inuyasha_selected, is_letitgo_selected, is_cannon_selected;    
}
