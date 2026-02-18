using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Refs (Inspector’dan atabilir ya da auto-wire)")]
    [SerializeField] private LetterSpawner spawner;
    [SerializeField] private WordBox wordBox;

    [Header("Level")]
    [SerializeField] private int level = 1;

    private readonly Queue<string> recent = new Queue<string>();
    private const int Keep = 30;

    void Awake()
    {
        // Dýþ sözlük (Resources/words_tr.txt) yüklensin
        TRWords.Load();

        // Oto-baðla (Inspector boþsa)
        if (!spawner) spawner = FindObjectOfType<LetterSpawner>(true);
        if (!wordBox) wordBox = FindObjectOfType<WordBox>(true);
    }

    void Start()
    {
        if (!spawner || !wordBox)
        {
            Debug.LogError("[RoundManager] Spawner veya WordBox bulunamadý. Inspector’dan sürükle veya sahnede var mý kontrol et.");
            return;
        }

        StartNewRound();
    }

    // Editörde deðer deðiþince de auto-wire dene
    void OnValidate()
    {
        if (!spawner) spawner = FindObjectOfType<LetterSpawner>(true);
        if (!wordBox) wordBox = FindObjectOfType<WordBox>(true);
    }

    [ContextMenu("Start New Round (Manual)")]
    public void StartNewRound()
    {
        var p = GetLevelParams(level);

        WordRound round;
        int guard = 0;
        do
        {
            round = WordRoundFactory.Create(p.min, p.max, p.dist);
        } while (recent.Contains(round.target) && guard++ < 20);

        recent.Enqueue(round.target);
        while (recent.Count > Keep) recent.Dequeue();

        spawner.StartRound(round); // LetterSpawner’daki kuyruða harfleri gönderir
        // Debug.Log($"Target: {round.target}");
    }

    (int min, int max, int dist) GetLevelParams(int lv)
    {
        if (lv < 3) return (3, 5, 2);
        if (lv < 6) return (4, 6, 3);
        if (lv < 10) return (5, 7, 4);
        return (6, 8, 5);
    }
}
