using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProceduralPop : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFakePop()
    {
        int sampleRate = 44100;
        float duration = 0.05f;
        int sampleLength = (int)(sampleRate * duration);
        float frequency = 900f;

        AudioClip clip = AudioClip.Create("PortPop", sampleLength, 1, sampleRate, false);
        float[] samples = new float[sampleLength];

        for (int i = 0; i < sampleLength; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate) * Mathf.Exp(-5f * i / (float)sampleLength);
        }

        clip.SetData(samples, 0);
        audioSource.PlayOneShot(clip, 500.0f);
    }
}
