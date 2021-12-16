using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller_Lee : MonoBehaviour
{
    TimingManager_Lee timingManager;
    // Start is called before the first frame update
    void Start()
    {
        timingManager = FindObjectOfType<TimingManager_Lee>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            timingManager.CheckTiming();
        }


        
    }
}
