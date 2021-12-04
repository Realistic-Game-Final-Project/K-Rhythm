using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyForPlayingGamePjw : MonoBehaviour
{
    private const int GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT = 3;
    private GameObject[] gayageum_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
    [SerializeField] private Button start_button;

    //일단은 가야금으로만 구현
    public void StartGame()
    {
        for (int i = 0; i < GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT; i++)
        {
            gayageum_sheet_canvas_childs[i] = GameObject.Find("Gayageum_Sheet_Canvas").transform.GetChild(i).gameObject;
            gayageum_sheet_canvas_childs[i].SetActive(true);
        }
        start_button.gameObject.SetActive(false);     
    }
}
