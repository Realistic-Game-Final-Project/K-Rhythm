using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectInstrumentPjw : MonoBehaviour
{
    private static SelectInstrumentPjw instance;
    public static SelectInstrumentPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SelectInstrumentPjw>();
            }
            return instance;
        }
    }

    private const int INSTRUMENTS_COUNT = 3;
     
    [SerializeField] private GameObject select_menu;
    [SerializeField] private Transform select_menu_transform;   
    [SerializeField] private GameObject announcement_text;    

    private Button[] instruments = new Button[INSTRUMENTS_COUNT];
    private Color non_transparent_color = new Color(255, 255, 255, 255);
    private Color transparent_color = new Color(255, 255, 255, 210);
    private const float BIGGER_SCALE = 1.1f;
    private const float DELAY_TIME = 3f;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < INSTRUMENTS_COUNT; i++)
        {
            instruments[i] = select_menu_transform.GetChild(i).GetComponent<Button>();
        }
    }

    public void SelectBanghyang()
    {
        Debug.Log("방향 선택");
        StaticDataPjw.is_banghyang_selected = true;
        StaticDataPjw.is_gayageum_selected = false;
        StaticDataPjw.is_janggu_selected = false;
        WorksAfterSelectInstrument();
    }
    public void SelectGayageum()
    {
        Debug.Log("가야금 선택");
        StaticDataPjw.is_banghyang_selected = false;
        StaticDataPjw.is_gayageum_selected = true;
        StaticDataPjw.is_janggu_selected = false;
        WorksAfterSelectInstrument();
    }
    public void SelectJanggu()
    {
        Debug.Log("장구 선택");
        StaticDataPjw.is_banghyang_selected = false;
        StaticDataPjw.is_gayageum_selected = false;
        StaticDataPjw.is_janggu_selected = true;
        WorksAfterSelectInstrument();
    }
 
    private void WorksAfterSelectInstrument()
    {
        select_menu.SetActive(false);
        announcement_text.SetActive(true);
        Invoke("Delay", DELAY_TIME);
    }

    private void Delay()
    {
        SceneManager.LoadScene("PlayGameScene");   
        //SceneManager.LoadScene("Test2");
    }
}
