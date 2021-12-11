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
    [SerializeField] private Sprite[] normal_ui = new Sprite[INSTRUMENTS_COUNT];
    [SerializeField] private Sprite[] transparent_ui = new Sprite[INSTRUMENTS_COUNT];
    [SerializeField] private GameObject announcement_text;

    private Button[] instruments = new Button[INSTRUMENTS_COUNT];
    private const float BIGGER_SCALE = 1.2f;
    private const float DELAY_TIME = 3f;

    private void Awake()
    {
        Initialize();
    }

    //형석씨가 요구한 코드
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) == true)
        {
            BecomeBiggerAndUntransparent(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            BecomeBiggerAndUntransparent(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            BecomeBiggerAndUntransparent(KeyCode.D);
        }
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
        Debug.Log("Banghyang");
        StaticDataPjw.is_banghyang_selected = true;
        StaticDataPjw.is_gayageum_selected = false;
        StaticDataPjw.is_banghyang_selected = false;
        WorksAfterSelectInstrument();
    }
    public void SelectGayageum()
    {
        Debug.Log("Gayageum");
        StaticDataPjw.is_banghyang_selected = false;
        StaticDataPjw.is_gayageum_selected = true;
        StaticDataPjw.is_janggu_selected = false;
        WorksAfterSelectInstrument();
    }
    public void SelectJanggu()
    {
        Debug.Log("Janggu");
        StaticDataPjw.is_banghyang_selected = false;
        StaticDataPjw.is_gayageum_selected = false;
        StaticDataPjw.is_janggu_selected = true;
        WorksAfterSelectInstrument();
    }

    //마우스가 올라가 있으면 이게 동작하고 , 없으면 동작 X 인데 우린 vr이라...
    //Test
    //A-0 , S-1 , D-2
    private void BecomeBiggerAndUntransparent(KeyCode keycode)
    {        
        if(keycode == KeyCode.A)
        {
            instruments[0].transform.localScale *= BIGGER_SCALE;
            instruments[0].image.sprite = normal_ui[0];
        }
        else if (keycode == KeyCode.S)
        {
            instruments[1].transform.localScale *= BIGGER_SCALE;
            instruments[1].image.sprite = normal_ui[1];
        }
        else if (keycode == KeyCode.D)
        {
            instruments[2].transform.localScale *= BIGGER_SCALE;
            instruments[2].image.sprite = normal_ui[2];
        }
    }

    private void BecomeNormal(KeyCode keycode)
    {
        Vector3 normal_vector = new Vector3(1, 1, 1);
        if (keycode == KeyCode.A)
        {
            instruments[0].transform.localScale = normal_vector;
            instruments[0].image.sprite = transparent_ui[0];
        }
        else if (keycode == KeyCode.S)
        {
            instruments[1].transform.localScale = normal_vector;
            instruments[1].image.sprite = transparent_ui[1];
        }
        else if (keycode == KeyCode.D)
        {
            instruments[2].transform.localScale = normal_vector;
            instruments[2].image.sprite = transparent_ui[2];
        }
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
    }
}
