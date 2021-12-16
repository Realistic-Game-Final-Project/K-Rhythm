using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager_Lee : MonoBehaviour
{
    private static TimingManager_Lee instance;
    public static TimingManager_Lee Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TimingManager_Lee>();
            }
            return instance;
        }
    }
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBox = null;


    EffectManager_Lee NEffect;

    private void Start()
    {
        //Effect Script Load
        NEffect = FindObjectOfType<EffectManager_Lee>();
        timingBox = new Vector2[timingRect.Length];
        for(int i = 0; i< timingRect.Length; i++)
        {
            timingBox[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2, Center.localPosition.x + timingRect[i].rect.width / 2); 
        }
    }

    public void CheckTiming()
    {
        for(int i = 0; i< boxNoteList.Count; i++)
        {
            float notePosX = boxNoteList[i].transform.localPosition.x;

            for(int x = 0; x<timingBox.Length; x++)
            {
                //Judgement Boundary set
                if(timingBox[x].x <= notePosX && notePosX <= timingBox[x].y)
                {
                    //Note Remove
                    boxNoteList[i].GetComponent<Note_Lee>().Hide();
                    boxNoteList.RemoveAt(i);

                    //Effect Set 0: Perfect 1: Good 2:Bad 3:Miss
                    if (x < timingBox.Length - 1)
                        NEffect.NoteHitEffect();
                    NEffect.JudgementEffect(x);
                    ScoreCount(x);
                    Debug.Log("perfect: " + perfect_count);
                    Debug.Log("great: " + great_count);
                    
                    return;
                }
            }
            
        }
        NEffect.JudgementEffect(3);

    }
    #region 점수 확인
    public int perfect_count;
    public int great_count;
    public int miss_count;
    public void ScoreCount(int x)
    {
        switch (x)
        {
            case 0:
                perfect_count++;
                break;
            case 1:
                great_count++;
                break;
            case 2:
                miss_count++;
                break;
        }


    }
    #endregion
}
