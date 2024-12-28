
public class PlayerData
{
    public int version = 0;

    public string displayName = "";
    public int colorIndex = 0;
    public int averageScore = 0;
    public int numOfRoundsEntered = 0;
    public int turnMusicIndex = 0;

    public PlayerData(string displayName, int colorIndex, int turnMusicIndex)
    {
        this.displayName = displayName;
        this.colorIndex = colorIndex;
        this.turnMusicIndex = turnMusicIndex;
    }
}
