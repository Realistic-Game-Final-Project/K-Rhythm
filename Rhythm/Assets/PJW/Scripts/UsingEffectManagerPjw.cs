using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingEffectManagerPjw : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("hit");
        EffectManager_Lee.Instance.NoteHitEffect();      
    }
   
}
