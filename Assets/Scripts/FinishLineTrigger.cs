using UnityEngine;
using DG.Tweening;

public class FinishLineTrigger : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float rotationAmount = 180f;

    [Header("Ses Efekti")]
    [SerializeField] private AudioSource boomAudioSource; // BOOM sesi için

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null || !other.gameObject || other.CompareTag("Letter") == false) return;

        LetterState state = other.GetComponent<LetterState>();
        if (state != null && state.hasBounced) return;
        if (state != null) state.hasBounced = true;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col = other.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer glowSr = other.GetComponentInChildren<SpriteRenderer>();
        Transform letterTransform = other.transform;

        // Harf pozisyonunu yukarı at
        float safeY = GetComponent<Collider2D>().bounds.max.y + 0.1f;
        if (letterTransform != null)
            letterTransform.position = new Vector3(letterTransform.position.x, safeY, 0);

        Vector3 jumpTarget = letterTransform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

        Sequence seq = DOTween.Sequence();

        // Zıplama + dönme
        if (letterTransform != null)
        {
            seq.Append(letterTransform.DOMove(jumpTarget, jumpDuration).SetEase(Ease.OutQuad));
            seq.Join(letterTransform.DORotate(new Vector3(0, 0, rotationAmount), jumpDuration, RotateMode.FastBeyond360));
        }

        // Glow fade out varsa
        if (glowSr != null)
            seq.Join(glowSr.DOFade(0, jumpDuration));

        seq.OnComplete(() =>
        {
            if (other != null && other.gameObject != null)
                SFXManager.PlayBoom();
            Destroy(other.gameObject);
        });

        // 💥 BOOM SESİ BURADA ÇALIŞIYOR
        if (boomAudioSource != null)
            boomAudioSource.Play();
    }
}
