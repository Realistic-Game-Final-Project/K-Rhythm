using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager_Lee : MonoBehaviour
{
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

                    //Effect Set
                    if (x < timingBox.Length - 1)
                        NEffect.NoteHitEffect();
                    NEffect.JudgementEffect(x);
                    
                    return;
                }
            }
            
        }
        NEffect.JudgementEffect(3);
    }
}
