using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager_Lee : MonoBehaviour
{
    private static EffectManager_Lee instance;
    public static EffectManager_Lee Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<EffectManager_Lee>();
            }
            return instance;
        }
    }

    [SerializeField] public Animator noteHitAni = null;
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
