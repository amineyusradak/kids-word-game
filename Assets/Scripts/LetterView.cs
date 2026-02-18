using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text label;
    public char Letter { get; private set; }
    WordBox wordBox;

    public void Init(char c, WordBox box)
    {
        Letter = c;
        wordBox = box;
        if (label != null) label.text = c.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (wordBox != null) wordBox.AddLetter(Letter.ToString());
        Destroy(gameObject);
    }
}
