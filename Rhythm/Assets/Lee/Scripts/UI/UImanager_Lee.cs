using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager_Lee : MonoBehaviour
{
    private static int UIscale = 6;
    [SerializeField]
    private GameObject[] UI = new GameObject[UIscale];
    private int NowUI = 0;
    [SerializeField]
    private GameObject[] Ist = new GameObject[3];

    public void Next()
    {

        UI[NowUI].SetActive(false);
        NowUI += 1;
        UI[NowUI].SetActive(true);
        if (NowUI == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                Ist[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                Ist[i].SetActive(false);
            }
        }
        //Debug.Log(NowUI);
    }

    public void GameSt()
    {
        SceneManager.LoadScene("Hyeongseok_Scene 1");
    }

    public void IntruSummon()
    {
        
    }
}
