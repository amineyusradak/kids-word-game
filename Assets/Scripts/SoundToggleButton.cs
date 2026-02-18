using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;
    public Image iconImage;
    public AudioSource backgroundMusic;

    private bool isSoundOn = true;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToggleSound);
        UpdateIcon();
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        backgroundMusic.volume = isSoundOn ? 1f : 0f;
        UpdateIcon();
    }

    void UpdateIcon()
    {
        iconImage.sprite = isSoundOn ? soundOnIcon : soundOffIcon;
    }
}
