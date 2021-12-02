using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (is_game_paused == false)
            {
                is_game_paused = true;
                PauseGame();
            }
            else
            {
                is_game_paused = false;
                RestartGame();
            }
        }
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
        //TODO : Event select particular instrument 
    }
    public void SelectGayageum()
    {
        Debug.Log("Gayageum");
        //TODO : Event select particular instrument 
    }
    public void SelectJanggu()
    {
        Debug.Log("Janggu");
        //TODO : Event select particular instrument 
    }
}
