using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMusicPjw : MonoBehaviour
{
    private const int TIME_COUNT = 4;
    private const int BUTTON_COUNT = 3;    
    private const int GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT = 3;
    private const int BANGHYANG_SHEET_CANVAS_CHILDS_COUNT = 3;
    [SerializeField] private const float DELAY_TIME = 1.5f;
    [SerializeField] private const int FONT_SIZE = 100;

    private GameObject[] gayageum_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
    private GameObject[] banghyang_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
   
    [SerializeField] private Canvas select_music_and_timer_canvas;
    [SerializeField] private Canvas gayageum_sheet_canvas, banghyang_sheet_canvas, janggu_sheet_canvas;
    [SerializeField] private Button[] music_buttons = new Button[BUTTON_COUNT];
    [SerializeField] private Text announcement_text;
        
    public void SelectInuyasha()
    {
        StaticDataPjw.is_inuyasha_selected = true;
        StaticDataPjw.is_letitgo_selected = false;
        StaticDataPjw.is_cannon_selected = false;
        DeactivateSelectMusicUI();
        TurnOffBackgroundMusic(); //게임 전체에서 나오는 배경음악 끄기
        StartCoroutine("Timer");        
    }
    public void SelectLetitgo()
    {
        StaticDataPjw.is_inuyasha_selected = false;
        StaticDataPjw.is_letitgo_selected = true;
        StaticDataPjw.is_cannon_selected = false;
        DeactivateSelectMusicUI();
        TurnOffBackgroundMusic();
        StartCoroutine("Timer");
    }
    public void SelectCannon()
    {
        StaticDataPjw.is_inuyasha_selected = false;
        StaticDataPjw.is_letitgo_selected = false;
        StaticDataPjw.is_cannon_selected = true;
        DeactivateSelectMusicUI();
        TurnOffBackgroundMusic();
        StartCoroutine("Timer");
    }

    private void TurnOffBackgroundMusic()
    {
        GameManagerPjw.Instance.StopAlwaysPlayingBackgroundMusicExceptGamePlaying();
    }
    private void DeactivateSelectMusicUI()
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
                yield return new WaitForSeconds(1f); //좀 더 쉼
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
                announcement_text.fontSize = FONT_SIZE;
            }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }
    
    //테스트 용 코드
    private void SelectTest()
    {
        StaticDataPjw.is_gayageum_selected = true;    
        //StaticDataPjw.is_banghyang_selected = true;       
        //StaticDataPjw.is_janggu_selected = true;
    }
    private void StartGame()
    {
        //SelectTest();
        if (StaticDataPjw.is_gayageum_selected == true)
        {
            Debug.Log("가야금 악보 활성화!");
            for (int i = 0; i < GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT; i++)
            {
                gayageum_sheet_canvas_childs[i] = GameObject.Find("Gayageum_Sheet_Canvas").transform.GetChild(i).gameObject;
                gayageum_sheet_canvas_childs[i].SetActive(true);
            }
        }
        else if(StaticDataPjw.is_banghyang_selected == true)
        {
            Debug.Log("방향 악보 활성화!");
            for (int i = 0; i < BANGHYANG_SHEET_CANVAS_CHILDS_COUNT; i++)
            {
                banghyang_sheet_canvas_childs[i] = GameObject.Find("Banghyang_Sheet_Canvas").transform.GetChild(i).gameObject;
                banghyang_sheet_canvas_childs[i].SetActive(true);
            }
        }
        else //장구 악보 활성화
        {
            Debug.Log("장구 악보 활성화");
            janggu_sheet_canvas.gameObject.SetActive(true);
        }
        DeactivateOtherMusicSheetCanvas();
        Invoke("DelayCallingMethod", DELAY_TIME); //악보 표출 후 1.5초 뒤에 실행   
    }
    
    private void DelayCallingMethod()
    {        
        BackgroundMusic.Instance.SelectMusicAndSaveStaticContainers(); //여기서 배경음악과 악보를 보고 플레이어가 치는 음악 2개를 모두 시작합니다.
    }
       
    private void DeactivateOtherMusicSheetCanvas() //각각의 악기 캔버스마다 악보에서 음악을 재생하는 코드가 존재하므로 , 선택되지 않은 객체는 끈다.
    {
        if (StaticDataPjw.is_banghyang_selected == true)
        {
            gayageum_sheet_canvas.gameObject.SetActive(false);
            //janggu_sheet_canvas.gameObject.SetActive(false);  
            //GanguSheetCanvas.SetActive(false);
        }
        else if (StaticDataPjw.is_gayageum_selected == true)
        {
            banghyang_sheet_canvas.gameObject.SetActive(false);
            //janggu_sheet_canvas.gameObject.SetActive(false);
            //GanguSheetCanvas.SetActive(false);
        }
        else if (StaticDataPjw.is_janggu_selected == true)
        {
            gayageum_sheet_canvas.gameObject.SetActive(false);            
            banghyang_sheet_canvas.gameObject.SetActive(false);
        }
    }
}
