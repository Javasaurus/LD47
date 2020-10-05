using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI myScore;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }
    }



    public void Pause()
    {
        ChadAudio.instance.PlayBreak();
        settingsPanel.SetActive(true);
        gamePanel.SetActive(false);
        isPaused = true;
        Time.timeScale = 0;
        myScore.text = "Current Score : " + ScoreManager.instance.score;
        highScore.text = "High Score    : " + Mathf.Max(ScoreManager.instance.score, PlayerPrefs.GetInt("HIGHSCORE"));
    }

    public void UnPause()
    {
        settingsPanel.SetActive(false);
        gamePanel.SetActive(true);
        isPaused = false;
        Time.timeScale = 1;
    }
}
