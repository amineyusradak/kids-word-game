using System.Collections.Generic;
using UnityEngine;

public class WordRound
{
    public string target;
    public List<char> letters = new List<char>(); // spawn edilecek harfler
}

public static class WordRoundFactory
{
    static System.Random rng = new System.Random();

    // Türkçe harf aðýrlýklarý (dikkat daðýtýcý için)
    static readonly (char c, int w)[] weights = new (char, int)[] {
        ('a',12),('e',9),('i',8),('ý',4),('n',7),('r',7),('l',6),
        ('k',5),('d',5),('t',5),('m',4),('u',4),('s',4),('y',4),
        ('o',3),('b',3),('ç',2),('g',2),('z',2),('h',2),('þ',2),
        ('p',2),('v',1),('ö',1),('ü',2),('ð',1)
    };

    public static WordRound Create(int minLen = 3, int maxLen = 7, int distractors = 3)
    {
        int L = Mathf.Clamp(Random.Range(minLen, maxLen + 1), minLen, maxLen);
        if (!TRWords.ByLen.ContainsKey(L)) L = 5;
        var set = TRWords.ByLen[L];

        string target = Pick(set);

        var wr = new WordRound { target = target };
        foreach (var ch in target) wr.letters.Add(ch);
        for (int i = 0; i < distractors; i++) wr.letters.Add(WeightedPick(weights));

        // karýþtýr
        for (int i = wr.letters.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (wr.letters[i], wr.letters[j]) = (wr.letters[j], wr.letters[i]);
        }
        return wr;
    }

    static string Pick(ICollection<string> set)
    {
        int idx = rng.Next(set.Count);
        foreach (var s in set) { if (idx-- == 0) return s; }
        return "araba";
    }

    static char WeightedPick((char c, int w)[] ws)
    {
        int total = 0; foreach (var t in ws) total += t.w;
        int r = rng.Next(total);
        foreach (var t in ws) { if (r < t.w) return t.c; r -= t.w; }
        return 'a';
    }
}
