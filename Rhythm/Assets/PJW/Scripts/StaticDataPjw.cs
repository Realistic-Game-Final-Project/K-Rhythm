using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//가야금의 음계를 자료구조에 저장 시키기 때문에 -1을 해줍니다.
enum GAYAGEUM_SCALE_NUMBER 
{
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

//방향도 자료구조에 저장 하므로 0부터 시작하므로 -1을 하고 
//기본적으로 샾,플랫이 없음
enum BANGHYANG_SCALE_NUMBER
{
    ONE = 0,
    THREE = 2,
    FIVE = 4,
    SIX = 5,
    EIGHT = 7,
    TEN = 9,
    TWELVE = 11,
    THIRTEEN = 12,
    FIFTEEN = 14
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
