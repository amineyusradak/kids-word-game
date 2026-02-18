using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class TRWords
{
    public static HashSet<string> All = new HashSet<string>();
    public static Dictionary<int, HashSet<string>> ByLen = new Dictionary<int, HashSet<string>>();
    static readonly CultureInfo tr = new CultureInfo("tr-TR");
    static bool loaded = false;

    public static void Load()
    {
        if (loaded) return;

        var ta = Resources.Load<TextAsset>("words_tr"); // Assets/Resources/words_tr.txt
        if (ta == null) { Debug.LogError("words_tr.txt bulunamadý (Assets/Resources)"); return; }

        var lines = ta.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var raw in lines)
        {
            var w = raw.Trim().ToLower(tr);
            if (w.Length < 3) continue;
            if (!OnlyTurkishLetters(w)) continue;

            All.Add(w);
            if (!ByLen.ContainsKey(w.Length)) ByLen[w.Length] = new HashSet<string>();
            ByLen[w.Length].Add(w);
        }

        loaded = true;
        Debug.Log($"TRWords loaded: {All.Count} kelime.");
    }

    static bool OnlyTurkishLetters(string s)
    {
        const string ok = "abcçdefgðhýijklmnoöprsþtuüvyz";
        foreach (var ch in s) if (ok.IndexOf(ch) < 0) return false;
        return true;
    }
}
