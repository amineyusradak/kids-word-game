using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Globalization;

public class LetterSpawner : MonoBehaviour
{
    public GameObject letterPrefab;
    public RectTransform spawnArea;
    public float fallSpeed = 150f;
    public float spawnInterval = 2f;
    public int lettersPerSpawn = 2;
    public int numberOfColumns = 5;
    public Transform letterParent;

    // 🔹 EKLEMELER: Round kuyruğu (istersen kapatmak için useRoundQueue)
    [SerializeField] private bool useRoundQueue = true;
    private readonly Queue<char> _roundQueue = new Queue<char>();
    static readonly CultureInfo tr = new CultureInfo("tr-TR");

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnLetters();
            timer = 0f;
        }
    }

    // 🔹 EKLEMELER: Dışarıdan tur başlatmak için (RoundManager vs.)
    public void StartRound(WordRound round)
    {
        _roundQueue.Clear();
        if (round == null || round.letters == null) return;

        foreach (var c in round.letters)
        {
            // Harfleri senin sistemin büyük harfle gösterdiği için TR-upper yapıyoruz
            var up = tr.TextInfo.ToUpper(c.ToString())[0];
            _roundQueue.Enqueue(up);
        }
    }

    // 🔹 EKLEMELER: Elle harf beslemek istersen
    public void FeedLetters(IEnumerable<char> letters)
    {
        foreach (var c in letters)
        {
            var up = tr.TextInfo.ToUpper(c.ToString())[0];
            _roundQueue.Enqueue(up);
        }
    }

    void SpawnLetters()
    {
        float canvasWidth = spawnArea.rect.width;
        float cellWidth = canvasWidth / numberOfColumns;
        List<int> usedColumns = new List<int>();

        for (int i = 0; i < lettersPerSpawn; i++)
        {
            int col;
            do
            {
                col = Random.Range(0, numberOfColumns);
            } while (usedColumns.Contains(col));
            usedColumns.Add(col);

            float xPos = -canvasWidth / 2 + cellWidth * col + cellWidth / 2;
            Vector3 spawnPos = spawnArea.position + new Vector3(xPos, 0f, 0f);

            GameObject newLetter = Instantiate(letterPrefab, spawnPos, Quaternion.identity, letterParent);

            // Türkçe harf ve sesli ağırlık (ORJİNALİN DURUYOR)
            string sesli = "AEIİOÖUÜ";
            string sessiz = "BCÇDFGĞHJKLMNPRSŞTVYZ";

            // 🔹 DEĞİŞİKLİK: Önce kuyruktan çek; boşsa eski rastgele mantık
            char harf;
            if (useRoundQueue && _roundQueue.Count > 0)
            {
                harf = _roundQueue.Dequeue();
            }
            else
            {
                harf = Random.value < 0.4f
                    ? sesli[Random.Range(0, sesli.Length)]
                    : sessiz[Random.Range(0, sessiz.Length)];
            }

            TMP_Text textComp = newLetter.GetComponentInChildren<TMP_Text>();
            if (textComp != null)
            {
                textComp.text = harf.ToString();
            }

            // 🔹 RENK EKLEME BAŞLANGIÇ (ORJİNALİN DURUYOR)
            UnityEngine.UI.Image bgImage = newLetter.GetComponentInChildren<UnityEngine.UI.Image>();
            if (bgImage != null)
            {
                Color balonRenk = GetRandomColor();
                bgImage.color = balonRenk;

                // Yazı rengi kontrast kontrolü
                float brightness = (balonRenk.r * 0.299f + balonRenk.g * 0.587f + balonRenk.b * 0.114f);
                if (textComp != null)
                    textComp.color = brightness < 0.5f ? Color.white : Color.black;
            }
            // 🔹 RENK EKLEME BİTİŞ (ORJİNAL)

            Rigidbody2D rb = newLetter.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(0, -fallSpeed);
            }
        }
    }

    Color GetRandomColor()
    {
        Color[] renkler = new Color[]
        {
            new Color32(255, 105, 180, 255),   // Pembe
            new Color32(173, 216, 230, 255),   // Bebek mavisi
            new Color32(255, 69, 58, 255),     // Kırmızı
            new Color32(255, 165, 0, 255),     // Turuncu
            new Color32(255, 192, 203, 255),   // Pastel pembe
            new Color32(135, 206, 235, 255)    // Sky blue
        };

        return renkler[Random.Range(0, renkler.Length)];
    }
}
