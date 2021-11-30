using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class Inuyasha_Lee
{
    public int scale;
    public float beat;
}
[System.Serializable]
public class MyTextDataArray_Lee
{
    public Inuyasha_Lee[] Inuyasha;
}


public class Music : MonoBehaviour
{
    TextAsset textdata;
    public MyTextDataArray_Lee mytext;
    public AudioClip[] audio;
    public AudioSource Mplayer;
    public AudioSource[] Mplayer1;
    public AudioSource[] Mplayer2;
    public int[] P_Beat;
    // Start is called before the first frame update
    void Awake()
    {
        
        StartCoroutine("JsonPar");
        //Mplayer.clip = audio[1];
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void Json()
    {
        textdata = Resources.Load("Inuyasha") as TextAsset;
        mytext = JsonUtility.FromJson<MyTextDataArray_Lee>(textdata.ToString());
        Debug.Log(mytext.Inuyasha[1].beat);
    }

    IEnumerator JsonPar()
    {
        textdata = Resources.Load("letitgo") as TextAsset;
        mytext = JsonUtility.FromJson<MyTextDataArray_Lee>(textdata.ToString());
        yield return new WaitForSeconds(3.0f);
        for(int i = 0; i< mytext.Inuyasha.Length; i++)
        {
            //0 -> Null
            if(mytext.Inuyasha[i].scale == 0)
            {
                Mplayer.clip = null;
                Mplayer1[0].Play();
                Mplayer.Play();
            }
            else
            {
               // 1 ³·Àº ½Ã 2 µµ ~~~~
               
                Mplayer.clip = audio[mytext.Inuyasha[i].scale-1];
                Mplayer1[mytext.Inuyasha[i].scale - 1].Play();
                if (mytext.Inuyasha[i].scale - 1 == 0)
                {
                    //Mplayer.pitch = 1 - 0.166f;
                }
                Mplayer.Play();
            }
            if (mytext.Inuyasha[i].beat >= 1.0f)
            {
                Mplayer2[0].Play();
                Mplayer2[0].time = 0.05f;
            }
            else if (mytext.Inuyasha[i].beat == 0.5f)
            {
                Mplayer2[1].Play();
                Mplayer2[1].time = 0.05f;
            }
            else if (mytext.Inuyasha[i].beat == 0.25f)
            {
                Mplayer2[2].Play();
                Mplayer2[2].time = 0.05f;
            }

            yield return new WaitForSeconds(mytext.Inuyasha[i].beat );
          //yield return new WaitForSeconds(mytext.Inuyasha[i].beat- 0.01f);
          // if(i< mytext.Inuyasha.Length -1)
          //Mplayer.pitch = 1+ Mathf.Lerp(1, (mytext.Inuyasha[i+1].scale - mytext.Inuyasha[i].scale) * 0.166f, 0.01f);
          //yield return new WaitForSeconds(0.01f);
          // Mplayer.pitch = 1;
            Mplayer.Stop();
            
        }
    }
}
