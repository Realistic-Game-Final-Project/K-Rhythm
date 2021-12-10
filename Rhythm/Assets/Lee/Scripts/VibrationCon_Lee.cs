using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationCon_Lee : MonoBehaviour
{
    public bool IsLeft;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("GayageumScale"))
        {
            //if(IsLeft)
                //OVRInput.SetControllerVibration(0.1f, 1, OVRInput.Controller.LTouch);
            //else
                //OVRInput.SetControllerVibration(0.1f, 1, OVRInput.Controller.RTouch);
        }
    }
}
