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
    private const float SPAWN_VALUE = 30f;

    private GameObject[] gayageum_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
    private GameObject[] banghyang_sheet_canvas_childs = new GameObject[GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT];
   
    [SerializeField] private Canvas select_music_and_timer_canvas;
    [SerializeField] private Canvas gayageum_sheet_canvas, banghyang_sheet_canvas, janggu_sheet_canvas;
    [SerializeField] private Button[] music_buttons = new Button[BUTTON_COUNT];
    [SerializeField] private Text announcement_text;
    [SerializeField] private GameObject banghyang, gayageum, janggu;

    public void SelectInuyasha()
    {
        StaticDataPjw.is_inuyasha_selected = true;
        StaticDataPjw.is_letitgo_selected = false;
        StaticDataPjw.is_cannon_selected = false;
        DeactivateSelectMusicUI();
        TurnOffBackgroundMusic(); //���� ��ü���� ������ ������� ����
        StartCoroutine(ConstructSelectedInstrument());
        StartCoroutine("Timer");        
    }
    public void SelectLetitgo()
    {
        StaticDataPjw.is_inuyasha_selected = false;
        StaticDataPjw.is_letitgo_selected = true;
        StaticDataPjw.is_cannon_selected = false;
        DeactivateSelectMusicUI();
        TurnOffBackgroundMusic();
        StartCoroutine(ConstructSelectedInstrument());
        StartCoroutine("Timer");
    }
    public void SelectCannon()
    {
        StaticDataPjw.is_inuyasha_selected = false;
        StaticDataPjw.is_letitgo_selected = false;
        StaticDataPjw.is_cannon_selected = true;
        DeactivateSelectMusicUI();
        TurnOffBackgroundMusic();
        StartCoroutine(ConstructSelectedInstrument());
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

    IEnumerator ConstructSelectedInstrument()
    {
        GameObject selected_instrument = null;
        if(StaticDataPjw.is_banghyang_selected == true)
        {
            selected_instrument = banghyang;
        }
        else if (StaticDataPjw.is_gayageum_selected == true)
        {
            selected_instrument = gayageum;
        }
        else if(StaticDataPjw.is_janggu_selected == true)
        {
            selected_instrument = janggu;
        }

        selected_instrument.SetActive(true);
        selected_instrument.transform.position += new Vector3(0, SPAWN_VALUE, 0);

        //2�� ���� ��������
        for(int i=0; i<SPAWN_VALUE; i++)
        {
            selected_instrument.transform.position -= new Vector3(0, 1, 0);
            yield return new WaitForSeconds(2f / SPAWN_VALUE);
        }
        yield return null;
    }

    IEnumerator Timer()
    {
        select_music_and_timer_canvas.transform.Find("AnnouncementAndTimer").gameObject.SetActive(true); //1ȸ�� �ڵ��̹Ƿ� ���⵵ �Ű� ���� ����
        for(int i=TIME_COUNT; i>=0; i--)
        {
            if(i==TIME_COUNT)
            {
                announcement_text.text = "������ �����ϼ̽��ϴ�.\n ������ �����ϰڽ��ϴ�!!!";
                yield return new WaitForSeconds(1f); //�� �� ��
            }
            else if(i==0)
            {
                announcement_text.text = "����!!!";
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
    
    //�׽�Ʈ �� �ڵ�
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
            Debug.Log("���߱� �Ǻ� Ȱ��ȭ!");
            for (int i = 0; i < GAYAGEUM_SHEET_CANVAS_CHILDS_COUNT; i++)
            {
                gayageum_sheet_canvas_childs[i] = GameObject.Find("Gayageum_Sheet_Canvas").transform.GetChild(i).gameObject;
                gayageum_sheet_canvas_childs[i].SetActive(true);
            }
        }
        else if(StaticDataPjw.is_banghyang_selected == true)
        {
            Debug.Log("���� �Ǻ� Ȱ��ȭ!");
            for (int i = 0; i < BANGHYANG_SHEET_CANVAS_CHILDS_COUNT; i++)
            {
                banghyang_sheet_canvas_childs[i] = GameObject.Find("Banghyang_Sheet_Canvas").transform.GetChild(i).gameObject;
                banghyang_sheet_canvas_childs[i].SetActive(true);
            }
        }
        else //�屸 �Ǻ� Ȱ��ȭ
        {
            Debug.Log("�屸 �Ǻ� Ȱ��ȭ");
            janggu_sheet_canvas.gameObject.SetActive(true);
        }
        DeactivateOtherMusicSheetCanvas();
        Invoke("DelayCallingMethod", DELAY_TIME); //�Ǻ� ǥ�� �� 1.5�� �ڿ� ����   
    }
    
    private void DelayCallingMethod()
    {        
        BackgroundMusic.Instance.SelectMusicAndSaveStaticContainers(); //���⼭ ������ǰ� �Ǻ��� ���� �÷��̾ ġ�� ���� 2���� ��� �����մϴ�.
    }
       
    private void DeactivateOtherMusicSheetCanvas() //������ �Ǳ� ĵ�������� �Ǻ����� ������ ����ϴ� �ڵ尡 �����ϹǷ� , ���õ��� ���� ��ü�� ����.
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
