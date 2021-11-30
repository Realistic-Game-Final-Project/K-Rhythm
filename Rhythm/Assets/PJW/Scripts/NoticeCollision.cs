using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeCollision : MonoBehaviour
{
    public const string banghyang_scale_tag = "BangHyangScale";
    public const string janggu_scale_tag = "JangguScale";
    public const string gayageum_scale_tag = "GayageumScale";

    public PlayBangHyang play_banghyang_script;
    public PlayJangGu play_janggu_script;
    public PlayGayageum play_gayageum_script;

    private void OnCollisionEnter(Collision collision)
    {
        ExamineInstrumentType(collision.gameObject.tag , collision.gameObject);
    }

    private void ExamineInstrumentType(string tag , GameObject scale_object)
    {
        if(tag == banghyang_scale_tag)
        {
            Debug.Log(tag + "  " + scale_object.name);
            play_banghyang_script.PlayInstrument(scale_object);
        }
        else if (tag == janggu_scale_tag)
        {
            /*
             * TODO : janggu
             */
        }
        else if (tag == gayageum_scale_tag)
        {
            Debug.Log(tag + "  " + scale_object.name);
            play_gayageum_script.PlayInstrument(scale_object);
        }
    }
}
