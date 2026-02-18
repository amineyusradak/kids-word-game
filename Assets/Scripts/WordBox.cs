using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Globalization;

public class WordBox : MonoBehaviour
{
    public static WordBox instance;
    [SerializeField] private TMP_Text wordBoxText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Color wrongWord;
    [SerializeField] private Color correctWord;
    public GameManager gameManager;

    // 🔹 EKLENTİ: Dış sözlük desteğini aç/kapat
    [SerializeField] private bool useExternalDictionary = true;
    // 🔹 EKLENTİ: İstersen kendi renk paletini kullan
    [SerializeField] private bool usePaletteColors = false;

    private string currentWord = "";
    private readonly CultureInfo tr = new CultureInfo("tr-TR");

    public void Start()
    {
        instance = this;
        if (useExternalDictionary)
        {
            // Assets/Resources/words_tr.txt varsa yükler
            TRWords.Load();
        }
    }

    // (ORİJİNAL) Sabit validWords – fallback olarak duruyor
    private HashSet<string> validWords = new HashSet<string>()
    {
        "merhaba", "oyun", "masa", "güneş", "kedi", "kitap", "araba", // Hayvanlar
        "aslan", "kaplan", "fil", "zürafa", "ayı", "köpek", "kedi", "tavşan", "ördek", "kuğu", "papağan", "at", "inek", "koyun", "keçi", "tilki", "penguen", "balina", "yunus", "maymun", "serçe", "kartal", "fare", "kaplumbağa",

        // Meyveler
        "elma", "armut", "muz", "çilek", "üzüm", "kavun", "karpuz", "portakal", "mandalina", "nar", "incir", "vişne", "kiraz", "dut", "ananas", "kivi", "avokado",

        // Sebzeler
        "havuç", "patates", "domates", "salatalık", "soğan", "sarımsak", "kabak", "lahana", "brokoli", "ıspanak", "bezelye", "fasulye", "biber", "pırasa", "marul",

        // Renkler
        "kırmızı", "mavi", "yeşil", "sarı", "turuncu", "mor", "pembe", "kahverengi", "siyah", "beyaz", "gri",

        // Meslekler
        "doktor", "öğretmen", "polis", "itfaiyeci", "mühendis", "hemşire", "pilot", "avukat", "çiftçi", "şoför", "fırıncı", "balıkçı", "veteriner", "astronot",

        // Duygular
        "mutlu", "üzgün", "kızgın", "heyecanlı", "şaşkın", "yorgun", "huzurlu", "neşeli", "korkmuş", "sevinçli",

        // Doğa & Diğer
        "güneş", "ay", "bulut", "yağmur", "kar", "yıldız", "orman", "deniz", "göl", "ırmak", "taş", "kum", "çiçek", "yaprak", "dağ", "rüzgar", "kuş", "ağaç",

        // Okul / Eğitim
        "kitap", "defter", "kalem", "silgi", "cetvel", "okul", "tahta", "ders", "ödev", "çanta", "masa", "sandalye", "sınıf", "zil", "harita",

        // Taşıtlar
    };

    public void AddLetter(string letter)
    {
        currentWord += letter;
        UpdateWordBoxText();
    }

    public void RemoveLastLetter()
    {
        if (currentWord.Length > 0)
        {
            currentWord = currentWord.Substring(0, currentWord.Length - 1);
            UpdateWordBoxText();
        }
    }

    public void CheckWord()
    {
        if (currentWord.Length >= 3)
        {
            // 🔹 DÜZELTME: Kontrolü temizlemeden ÖNCE yap
            string wordToCheck = currentWord.ToLower(tr);

            bool isValid;
            if (useExternalDictionary && TRWords.All.Count > 0)
                isValid = TRWords.All.Contains(wordToCheck);
            else
                isValid = validWords.Contains(wordToCheck);

            if (isValid)
            {
                feedbackText.text = "DOĞRU KELİME";
                feedbackText.color = usePaletteColors ? correctWord : Color.green;
                if (gameManager != null) gameManager.AddScore(10);
            }
            else
            {
                feedbackText.text = "YANLIŞ KELİME";
                feedbackText.color = usePaletteColors ? wrongWord : Color.red;
                if (gameManager != null) gameManager.AddScore(-5);
            }

            // Orijinal akış: temizle + geri sayım
            currentWord = "";
            UpdateWordBoxText();
            CancelInvoke();
            Invoke("ClearFeedback", 2f);
        }
        else
        {
            feedbackText.text = "En az 3 harf gir!";
            feedbackText.color = Color.yellow;
            CancelInvoke();
            Invoke("ClearFeedback", 2f);
        }
    }

    private void ClearFeedback()
    {
        feedbackText.text = "";
    }

    private void UpdateWordBoxText()
    {
        wordBoxText.text = currentWord;
    }
}
