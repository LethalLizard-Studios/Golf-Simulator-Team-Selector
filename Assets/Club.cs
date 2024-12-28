using UnityEngine;

public class Club
{
    public string name;
    public Vector2[] playerShots;
    public string bestPlayerName;
    public int bestPlayerResult;

    public Club(string name, int numOfPlayers)
    {
        this.name = name;

        playerShots = new Vector2[numOfPlayers];
        bestPlayerName = "";
        bestPlayerResult = 0;
    }
}
