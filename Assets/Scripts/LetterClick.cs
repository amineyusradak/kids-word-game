using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class LetterClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text letterText;

    public void OnPointerClick(PointerEventData eventData)
    {
        WordBox.instance.AddLetter(letterText.text);

        // 💥 Harfe basınca "port" sesi
        FindObjectOfType<ProceduralPop>().PlayFakePop();

        // 🔁 Animasyon varsa iptal et (yoksa hata verir)
        if (DOTween.IsTweening(transform))
            DOTween.Kill(transform);

        // ❌ GameObject'i sahneden sil
        Destroy(gameObject);
    }
}