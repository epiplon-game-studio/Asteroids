using UnityEngine;
using UnityEngine.UI;

public class UIHighscore : MonoBehaviour
{
    Text message;

    private void Awake()
    {
        message = GetComponent<Text>();
    }

    public void SetMessage(int position, HighscoreEntry entry)
    {
        message.text = string.Format("{0}: {1} - {2}", FormatRank(position + 1), entry.Name, entry.Points);
    }

    public string FormatRank(int value)
    {
        if (value == 1)
            return "1st";
        if (value == 2)
            return "2nd";
        if (value == 3)
            return "3rd";

        return value + "th";
    }
}
