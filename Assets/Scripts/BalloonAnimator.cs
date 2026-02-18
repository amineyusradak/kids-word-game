using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BalloonAnimator : MonoBehaviour
{
    private Image bgImage;
    private RectTransform rectTransform;
    private TMP_Text text;

    public float fallTime = 10f; // Daha yavaş iniş için artırıldı
    public float swingAmount = 30f;
    public float swingSpeed = 2f;

    private Tween swingTween;
    private Tween fallTween;
    private Sequence glowSequence;

    void Start()
    {
        bgImage = transform.Find("BalloonBackground")?.GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<TMP_Text>();

        if (bgImage != null)
        {
            Color renk = GetRandomColor();
            bgImage.color = renk;

            float brightness = renk.r * 0.299f + renk.g * 0.587f + renk.b * 0.114f;
            if (text != null)
                text.color = brightness < 0.5f ? Color.white : Color.black;

            // Daha belirgin parıltı efekti
            glowSequence = DOTween.Sequence();
            glowSequence.Append(bgImage.DOFade(1f, 0.3f));   // Parlak
            glowSequence.Append(bgImage.DOFade(0.7f, 0.3f)); // Soluk
            glowSequence.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        if (rectTransform != null)
        {
            float startX = rectTransform.anchoredPosition.x;

            // Salınım efekti (sağ-sol)
            swingTween = rectTransform.DOAnchorPosX(startX + swingAmount, swingSpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            // Aşağı doğru süzülme
            fallTween = rectTransform.DOAnchorPosY(-Screen.height, fallTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    KillTweens();
                    Destroy(gameObject);
                });
        }
    }

    void KillTweens()
    {
        if (glowSequence != null) glowSequence.Kill();
        if (swingTween != null) swingTween.Kill();
        if (fallTween != null) fallTween.Kill();
    }

    void OnDestroy()
    {
        KillTweens();
    }

    Color GetRandomColor()
    {
        Color[] renkler = new Color[]
        {
            new Color32(255, 105, 180, 255),   // Pembe
            new Color32(173, 216, 230, 255),   // Bebe Mavisi
            new Color32(255, 69, 58, 255),     // Kırmızı
            new Color32(255, 165, 0, 255),     // Turuncu
            new Color32(255, 192, 203, 255),   // Açık Pembe
            new Color32(135, 206, 235, 255),   // Sky Blue
            new Color32(144, 238, 144, 255),   // Açık Yeşil
            new Color32(255, 222, 173, 255)    // Lemon
        };

        return renkler[Random.Range(0, renkler.Length)];
    }
}

