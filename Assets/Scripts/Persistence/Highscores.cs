using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Highscores
{
    public HighscoreEntry[] Entries;

    public Highscores()
    {
        Entries = new HighscoreEntry[6]; // max 6 scores
    }

    /// <summary>
    /// Creates a new highscore
    /// </summary>
    /// <param name="name">Player name</param>
    /// <param name="points">Achieved points</param>
    public void New(string name, int points)
    {
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
