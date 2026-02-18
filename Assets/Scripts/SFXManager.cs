using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource boomSource;

    private static SFXManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void PlayBoom()
    {
        if (instance != null && instance.boomSource != null)
        {
            instance.boomSource.PlayOneShot(instance.boomSource.clip);
        }
    }
}
