using UnityEngine;

[System.Serializable]
public class Account
{
    public string username;
    public string identification;
    public string musicKit;
    public Color preferredColor;

    public int rank;
    public int goldsWon;
    public int silversWon;
    public int bronzesWon;

    public int[] clubsTested;
    public int[] clubScore;
    public string tipToWorkOn;

    public Account(string username, string identification, string musicKit, Color preferredColor)
    {
        this.username = username;
        this.identification = identification;
        this.musicKit = musicKit;
        this.preferredColor = preferredColor;

        rank = 0;
        goldsWon = 0;
        silversWon = 0;
        bronzesWon = 0;
    }
}
