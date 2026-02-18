using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public TMP_Text gameOverMessage;
    public GameObject restartButton; // atanmasa da sorun yok

    private float timeRemaining = 120f;
    private int score = 0;
    private bool isGameOver = false;

    void Start()
    {
        UpdateScoreText();
        UpdateTimerText();
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (restartButton) restartButton.SetActive(false); // buton yoksa dokunma
    }

    void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerText();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText) scoreText.text = "Puan:" + score;
    }

    private void UpdateTimerText()
    {
        if (timerText) timerText.text = "Süre:" + Mathf.CeilToInt(timeRemaining).ToString();
    }

    private void EndGame()
    {
        isGameOver = true;
        if (gameOverPanel) gameOverPanel.SetActive(true);
        if (gameOverMessage) gameOverMessage.text = "OYUN BİTTİ";

        // Restart butonu varsa 2 sn sonra göster; yoksa hiçbir şey yapma
        if (restartButton) Invoke(nameof(ShowRestartButton), 2f);
    }

    private void ShowRestartButton()
    {
        if (!restartButton) return; // buton yoksa çık
        if (gameOverMessage) gameOverMessage.text = ""; // yazıyı sil
        restartButton.SetActive(true); // butonu göster
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
