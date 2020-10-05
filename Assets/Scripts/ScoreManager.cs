using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public List<Fence> fences;
    public int damagedFences;

    public static bool GAMEOVER;

    public delegate void OnGameOver();
    public OnGameOver onGameOver;

    public int multiplier = 1;

    public TextMeshProUGUI textField;
    public Animator textAnimator;
    public Explosion explosion;
    public GameObject GameOverButton;

    private void Awake()
    {
        instance = this;
        fences = new List<Fence>();
        fences.AddRange(GameObject.FindObjectsOfType<Fence>());
        GAMEOVER = false;
    }

    void Start()
    {
        ShowMessage("Make your last stand !");
    }

    private void Update()
    {
        damagedFences = fences.Count(fence => fence.Health <= 0);
    }

    public GameObject scoreIndicatorPrefab;
    public TMPro.TextMeshProUGUI scoreField;
    private int _score;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;

            scoreField.text = "$" + value;
        }
    }

    public void AddScore( int value, Color color, Vector3 offset, Transform owner = null )
    {
        int increment = value * multiplier;
        score += increment;
        AddMessage("" + value, color, offset, owner);
    }

    public void AddMessage( string value, Color color, Vector3 offset, Transform owner = null )
    {
        if (owner != null)
        {
            GameObject scoreIndicatorInstance = GameObject.Instantiate(scoreIndicatorPrefab);
            scoreIndicatorInstance.transform.position = owner.position + offset;
            TextMeshProUGUI textField = scoreIndicatorInstance.GetComponentInChildren<TextMeshProUGUI>();
            textField.color = color;
            textField.text = value;
        }
    }


    public void GameOver()
    {
        if (GAMEOVER) return;
        ChadAudio.instance.PlayGameOver();
        PlayerPrefs.SetInt("HIGHSCORE", score);
        onGameOver?.Invoke();
        textField.text = "GAME OVER";
        textAnimator.SetBool("FinalizeText", true);
        GAMEOVER = true;
        explosion.Explode();
        GameOverButton.SetActive(true);
    }

    public void ShowMessage( string message )
    {
        textField.text = message;
        textAnimator.SetTrigger("DisplayText");
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

}
