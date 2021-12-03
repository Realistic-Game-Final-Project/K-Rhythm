using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerPjw : MonoBehaviour
{
    private static ScoreManagerPjw instance;
    public static ScoreManagerPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ScoreManagerPjw>();
            }
            return instance;
        }
    }
    enum SCORE
    {
        PERFECT = 5, GREAT = 3,  MISS = -2
    }
    private const int SCORE_TYPES_COUNT = 3;
   
    [SerializeField] Text[] perfect_great_miss_count = new Text[SCORE_TYPES_COUNT];
    [SerializeField] Text sum_of_score;

    //게임 끝나면 호출될 것.
    public void BecomeActivate()
    {
        transform.GetChild(0).gameObject.SetActive(true);       
    }

    public void BecomeDeActivate()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void MeasureScore(int perfect_count , int great_count , int miss_count)
    {        
        perfect_great_miss_count[0].text = perfect_count.ToString() + "개";
        perfect_great_miss_count[1].text = great_count.ToString() + "개";
        perfect_great_miss_count[2].text = miss_count.ToString() + "개";

        int tmp = 0;
        tmp += (perfect_count * (int)SCORE.PERFECT) + (great_count * (int)SCORE.GREAT) + (miss_count * (int)SCORE.MISS);
        sum_of_score.text = tmp + "점";
    }
}
