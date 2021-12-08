using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerPjw : MonoBehaviour
{
    private static UIManagerPjw instance;
    public static UIManagerPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManagerPjw>();
            }
            return instance;
        }
    }

    private const int INSTRUMENTS_COUNT = 3;

    [SerializeField]
    private Image pause_image;
    [SerializeField]
    private GameObject select_menu;
    [SerializeField]
    private Transform select_menu_transform;
    private Button[] instruments = new Button[INSTRUMENTS_COUNT];
    private bool is_game_paused = false;

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

    private void PauseGame()
    {
        pause_image.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    private void RestartGame()
    {
        pause_image.gameObject.SetActive(false);
        Time.timeScale = 1;    
    }

    public void SelectBanghyang()
    {
        Debug.Log("Banghyang");
        StaticDataPjw.is_banghyang_selected = true;
        StaticDataPjw.is_gayageum_selected = false;
        StaticDataPjw.is_banghyang_selected = false;
        SceneManager.LoadScene("SelectedInstrumentScene");       
    }
    public void SelectGayageum()
    {
        Debug.Log("Gayageum");
        StaticDataPjw.is_banghyang_selected = false;
        StaticDataPjw.is_gayageum_selected = true;
        StaticDataPjw.is_janggu_selected = false;
        SceneManager.LoadScene("SelectedInstrumentScene");
    }
    public void SelectJanggu()
    {
        Debug.Log("Janggu");
        StaticDataPjw.is_banghyang_selected = false;
        StaticDataPjw.is_gayageum_selected = false;
        StaticDataPjw.is_janggu_selected = true;
        SceneManager.LoadScene("SelectedInstrumentScene");
    }
}
