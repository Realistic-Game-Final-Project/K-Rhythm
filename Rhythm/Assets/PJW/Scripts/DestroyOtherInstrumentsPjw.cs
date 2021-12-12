using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherInstrumentsPjw : MonoBehaviour
{
    private GameObject banghyang, janggu, gayageum;
    private Vector3 middle_position = new Vector3(369.7f, 94.8f, 524.8f); //기물을 위치시킬 위치
    
    private void Awake()
    {   
        Initialize();
        SaveonlySelectedInstrument();
    }

    private void SaveonlySelectedInstrument()
    {
        if (StaticDataPjw.is_banghyang_selected == true) //ERROR : 얘만 위에서 debug를 해보면 true , true , true로 이유를 모르게 바뀌지 않음
        {
            Destroy(janggu);
            Destroy(gayageum);
            banghyang.transform.position = middle_position;
        }        
        else if (StaticDataPjw.is_gayageum_selected == true)
        {
            Destroy(banghyang);
            Destroy(janggu);
            gayageum.transform.position = middle_position;
        }
        else if (StaticDataPjw.is_janggu_selected == true)
        {
            Destroy(banghyang);
            Destroy(gayageum);
            janggu.transform.position = middle_position;
        }
    }
    private void Initialize()
    {
        banghyang = GameObject.FindGameObjectWithTag("Banghyang");
        janggu = GameObject.FindGameObjectWithTag("Janggu");
        gayageum = GameObject.FindGameObjectWithTag("Gayageum");
    }

}
