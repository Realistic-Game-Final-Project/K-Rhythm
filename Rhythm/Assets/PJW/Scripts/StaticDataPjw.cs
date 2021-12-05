using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GAYAGEUM_SCALE_NUMBER //가야금 음을 한국말로 모름 
{
    ONE = 3,
    TWO = 4,
    THREE = 5,
    FOUR = 6,
    FIVE = 7,
    SIX = 8,
    SEVEN = 9,
    EIGHT = 10,
    NINE = 11
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
