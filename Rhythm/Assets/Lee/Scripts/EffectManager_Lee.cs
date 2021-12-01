using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager_Lee : MonoBehaviour
{
    [SerializeField] Animator noteHitAni = null;
    string hit = "Hit";
    [SerializeField] Animator JudgeAni = null;
    [SerializeField] UnityEngine.UI.Image judgeImage = null;
    [SerializeField] Sprite[] judgeSprite = null;

    public void JudgementEffect(int p_num)
    {
        //Perfect, Good, Bad, Miss
        judgeImage.sprite = judgeSprite[p_num];
        JudgeAni.SetTrigger(hit);
    }

    public void NoteHitEffect()
    {
        //Note Animation Set
        noteHitAni.SetTrigger(hit);
    }
}
