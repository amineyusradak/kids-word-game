using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject blurPanel;
    public Button pauseButton;
    public Button restartButton;
    public Button soundToggleButton;
    public Button menuToggleButton;
    public Button closeButton; // Menü içindeki geri/kapat butonu

    private bool isMenuOpen = false;
    private bool isSoundOn = true;

    void Start()
    {
        // Butonlara listener ekle
        if (menuToggleButton != null)
            menuToggleButton.onClick.AddListener(OpenMenu);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (soundToggleButton != null)
            soundToggleButton.onClick.AddListener(ToggleSound);

        // Menü baþlangýçta kapalý
        menuPanel.SetActive(false);
        blurPanel.SetActive(false);
    }

    void OpenMenu()
    {
        isMenuOpen = true;
        menuPanel.SetActive(true);
        blurPanel.SetActive(true);
        Time.timeScale = 0f; // Oyunu dondur
    }

    void CloseMenu()
    {
        isMenuOpen = false;
        menuPanel.SetActive(false);
        blurPanel.SetActive(false);
        Time.timeScale = 1f; // Oyunu devam ettir
    }

    void TogglePause()
    {
        Time.timeScale = (Time.timeScale == 0f) ? 1f : 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.volume = isSoundOn ? 1f : 0f;
    }
}
