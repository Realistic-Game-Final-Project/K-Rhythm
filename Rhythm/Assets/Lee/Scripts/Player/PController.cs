using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController : MonoBehaviour
{
    [SerializeField]
    private GameObject GanguCha;
    [SerializeField]
    private GameObject BangHangCha;
    [SerializeField]
    private GameObject JangguHand;
    [SerializeField]
    private GameObject RGaHand;
    [SerializeField]
    private GameObject LGaHand;
    [SerializeField]
    private Animator LHand;
    [SerializeField]
    private Animator RHand;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Banghyang")
        {
            BangHangCha.SetActive(true);
            LHand.SetBool("Hold", true);
            
        }
        else if(other.gameObject.tag == "Janggu")
        {
            GanguCha.SetActive(true);
            JangguHand.SetActive(true);
            RHand.SetBool("Hold", true);
        }
        else if(other.gameObject.tag == "Gayageum")
        {
            LGaHand.SetActive(true);
            RGaHand.SetActive(true);
            LHand.SetBool("Ga", true);
            RHand.SetBool("Ga", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Banghyang")
        {
            BangHangCha.SetActive(false);
            LHand.SetBool("Hold", false);
        }
        else if (other.gameObject.tag == "Janggu")
        {
            GanguCha.SetActive(false);
            JangguHand.SetActive(false);
            RHand.SetBool("Hold", false);
        }
        else if (other.gameObject.tag == "Gayageum")
        {
            LGaHand.SetActive(false);
            RGaHand.SetActive(false);
            LHand.SetBool("Ga", false);
            RHand.SetBool("Ga", false);
        }
    }
}
