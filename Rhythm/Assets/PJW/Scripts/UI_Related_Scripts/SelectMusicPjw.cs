using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMusicPjw : MonoBehaviour
{
    private const int TIME_COUNT = 4;
    private const int BUTTON_COUNT = 3;
    private const int BANGHYANG_SHEET_CANVAS_CHILDS_COUNT = 2;
    private const int GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT = 3;
    private GameObject[] gayageum_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
    private GameObject[] banghyang_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
    [SerializeField] private Canvas select_music_and_timer_canvas;
    [SerializeField] private Button[] music_buttons = new Button[BUTTON_COUNT];
    [SerializeField] private Text announcement_text;
    
    public void SelectInuyasha()
    {
        StaticDataPjw.is_inuyasha_selected = true;
        StaticDataPjw.is_letitgo_selected = false;
        StaticDataPjw.is_cannon_selected = false;
        DeactivateSelectMusics();
        StartCoroutine("Timer");        
    }
    public void SelectLetitgo()
    {
        StaticDataPjw.is_inuyasha_selected = false;
        StaticDataPjw.is_letitgo_selected = true;
        StaticDataPjw.is_cannon_selected = false;
        DeactivateSelectMusics();
        StartCoroutine("Timer");
    }
    public void SelectCannon()
    {
        StaticDataPjw.is_inuyasha_selected = false;
        StaticDataPjw.is_letitgo_selected = false;
        StaticDataPjw.is_cannon_selected = true;
        DeactivateSelectMusics();
        StartCoroutine("Timer");
    }

    private void DeactivateSelectMusics()
    {
        for(int i=0; i<BUTTON_COUNT; i++)
        {
            music_buttons[i].gameObject.SetActive(false);
        }
    }
    IEnumerator Timer()
    {
        select_music_and_timer_canvas.transform.Find("AnnouncementAndTimer").gameObject.SetActive(true); //1회성 코드이므로 복잡도 신경 쓰지 않음
        for(int i=TIME_COUNT; i>=0; i--)
        {
            if(i==TIME_COUNT)
            {
                announcement_text.text = "음악을 선택하셨습니다.\n 게임을 시작하겠습니다!!!";
            }
            else if(i==0)
            {
                announcement_text.text = "시작!!!";
                yield return new WaitForSeconds(0.8f);
                select_music_and_timer_canvas.transform.Find("AnnouncementAndTimer").gameObject.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                StartGame();
                yield return null;
            }
            else
            {
                announcement_text.text = i.ToString();
            }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }
    

    //일단은 가야금으로만 구현
    //static으로 찾아서 변경
    private void StartGame()
    {
        // TEST CODE
        // 악기 선택하는 부분을 스킵하기 위해 테스트 코드        
        //1. 가야금 선택
        StaticDataPjw.is_gayageum_selected = true;
        //2. 방향 선택
        //StaticDataPjw.is_banghyang_selected = true;

        if (StaticDataPjw.is_gayageum_selected == true)
        {
            for (int i = 0; i < GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT; i++)
            {
                gayageum_sheet_canvas_childs[i] = GameObject.Find("Gayageum_Sheet_Canvas").transform.GetChild(i).gameObject;
                gayageum_sheet_canvas_childs[i].SetActive(true);
            }
        }
        else if(StaticDataPjw.is_banghyang_selected == true)
        {
            Debug.Log("bang향");
            for (int i = 0; i < BANGHYANG_SHEET_CANVAS_CHILDS_COUNT; i++)
            {
                gayageum_sheet_canvas_childs[i] = GameObject.Find("Banghyang_Sheet_Canvas").transform.GetChild(i).gameObject;
                gayageum_sheet_canvas_childs[i].SetActive(true);
            }
        }
        BackgroundMusic.Instance.SelectMusicAndSaveStaticContainers();
    }
}
