using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public TextMeshProUGUI highscoreField;

    void Start()
    {
        int score = PlayerPrefs.GetInt("HIGHSCORE", 0);
        highscoreField.text = score > 0 ? "High Score : $" + score : "";
    }


    public void ShowMenu()
    {
        menuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void ShowSettings()
    {
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
