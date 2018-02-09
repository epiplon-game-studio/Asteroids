using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Highscores
{
    const string FILENAME = "highscores.meteor";

    public HighscoreEntry[] Entries;

    public Highscores()
    {
        Entries = new HighscoreEntry[6]; // max 6 scores
        for (int i = 0; i < Entries.Length; i++)
            Entries[i] = new HighscoreEntry("VINICIUS", 0000000100);
    }

    /// <summary>
    /// Creates a new highscore, decreasing order in the array.
    /// </summary>
    /// <param name="name">Player name</param>
    /// <param name="points">Achieved points</param>
    public void New(string name, int points)
    {
        name = string.IsNullOrEmpty(name) ? "Anonymous" : name;
        var newEntry = new HighscoreEntry(name, points);
        for (int i = 0; i < Entries.Length; i++)
        {
            if(Entries[i].Points == 0)
            {
                Entries[i] = newEntry;
                return;
            }
            if (newEntry.Points > Entries[i].Points)
            {
                if(i == Entries.Length -1)
                {
                    Entries[i] = newEntry;
                    return;
                }

                HighscoreEntry nextEntry = newEntry;
                HighscoreEntry temp;
                for (int j = i; j < Entries.Length; j++)
                {
                    temp = Entries[j];
                    Entries[j] = nextEntry;
                    nextEntry = temp;
                }
                return;
            }
        }
    }

    public static Highscores Load()
    {
        var filepath = System.IO.Path.Combine(Application.persistentDataPath, FILENAME);
        if (System.IO.File.Exists(filepath))
        {
            string json = System.IO.File.ReadAllText(filepath);
            return JsonUtility.FromJson<Highscores>(json);
        }
        else
        {
            var newHighscores = new Highscores();
            string highJson = JsonUtility.ToJson(newHighscores);
            System.IO.File.WriteAllText(filepath, highJson);
            return newHighscores;
        }
    }

    public void Save()
    {
        var filepath = System.IO.Path.Combine(Application.persistentDataPath, FILENAME);
        string highJson = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(filepath, highJson);
    }
}

[System.Serializable]
public struct HighscoreEntry
{
    public string Name;
    public int Points;
        
    public HighscoreEntry(string name, int points)
    {
        Name = name;
        Points = points;
    }
}
