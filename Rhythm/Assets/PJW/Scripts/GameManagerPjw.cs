using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPjw : MonoBehaviour
{
    [SerializeField] private Canvas score_canvas;
    [SerializeField] private Image pause_image;
    public bool is_game_paused = false;
    public bool is_game_ended = false;

    private static GameManagerPjw instance;
    public static GameManagerPjw Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManagerPjw>();
            }
            return instance;
        }
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

    public void ShowScoreBoard()
    {
        score_canvas.gameObject.SetActive(true);
        ScoreManagerPjw.Instance.BecomeActivate();
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
}
