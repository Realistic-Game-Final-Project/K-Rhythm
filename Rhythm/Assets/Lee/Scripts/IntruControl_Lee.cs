using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntruControl_Lee : MonoBehaviour
{
    public const string banghyang_scale_tag = "BangHyangScale";
    public const string janggu_scale_tag = "JangGuScale";
    public const string gayageum_scale_tag = "GayageumScale";
    public const string yonggo_scale_tag = "YonggoScale";

    public PlayBangHyang play_banghyang_script;
    public PlayJangGu play_janggu_script;
    public PlayGayageum play_gayageum_script;
    public PlayYonggo play_yonggo_script;

    private void OnTriggerEnter(Collider collision) 
    {
        ExamineInstrumentType(collision.gameObject.tag, collision.gameObject);
    }

    private void ExamineInstrumentType(string tag, GameObject scale_object)
    {
        bool is_tuto = false;
        if(SceneManager.GetActiveScene().name == "Tuto")
        {
            is_tuto = true;
        }
        if (tag == banghyang_scale_tag)
        {
            if(is_tuto == true)
            {
                play_banghyang_script.PlayInstrument(scale_object);
            }
            else
            {
                RhythmGameOnBanghyangPjw.Instance.CheckInputs(scale_object.name);
            }                 
        }
        else if (tag == janggu_scale_tag)
        {
            if (is_tuto == true)
            {
                play_janggu_script.PlayInstrument(scale_object);
            }
            else
            {
                //TODO : 
            }
        }
        else if (tag == gayageum_scale_tag)
        {
            if (is_tuto == true)
            {
                play_gayageum_script.PlayInstrument(scale_object);
            }
            else
            {
                RhythmGameOnSelectedSheetPjw.Instance.CheckInputs(scale_object.name);
            }                     
        }
        else if(tag == yonggo_scale_tag)
        {
            play_yonggo_script.PlayInstrument(scale_object);
        }
    }
}
