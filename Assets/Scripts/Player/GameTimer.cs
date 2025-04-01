using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public InputField timeInput;
    public GameObject gameOverUI;
    public GameObject winUI;

    private float timeRemaining;
    private bool isGameOver = false;
    private bool gameStarted = false;

    void Start()
    {
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
    }

    public void StartGame()
    {
        if (float.TryParse(timeInput.text, out timeRemaining))
        {
            gameStarted = true;
            timeInput.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (gameStarted && !isGameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = "Time: " + timeRemaining.ToString("0.0");
            }
            else
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0; // Dừng game
    }

    public void WinGame()
    {
        isGameOver = true;
        winUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
