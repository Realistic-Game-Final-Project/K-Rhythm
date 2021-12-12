using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingEffectManagerPjw : MonoBehaviour
{
    private void Awake()
    {
        //Debug.Log(transform.position + "  " + gameObject.name);              
    }

    private void FixedUpdate()
    {
        EffectManager_Lee.Instance.NoteHitEffect();
    }
}
