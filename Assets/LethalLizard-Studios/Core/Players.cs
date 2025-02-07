using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public static Players Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    [SerializeField] private InputShot inputShot;
    [SerializeField] private ActiveAccounts activeAccounts;
    [SerializeField] private Recap recap;
    [SerializeField] private GameObject teamsScreen;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform redParent;

    public int numberOfPlayers = 0;
    public List<PlayerData> playerData = new List<PlayerData>();
    public List<int> playerScore = new List<int>();

    public InputShot shot;
    public List<Club> clubs = new List<Club>();

    [SerializeField] private Color[] poduimFinishColor = new Color[3];
    [SerializeField] private Color[] teamColors = new Color[2];

    [SerializeField] private ClubSelection clubSelection;

    public void OnEnable()
    {
        AddAllPlayers(activeAccounts.GetAllAcounts());
    }

    public void AddAllPlayers(Account[] accounts)
    {
        clubs.Clear();
        playerData.Clear();

        numberOfPlayers = accounts.Length;

        for (int i = 0; i < clubSelection.GetClubCount(); i++)
        {
            clubs.Add(new Club(clubSelection.GetClub(i).GetDisplayName(), numberOfPlayers));
        }

        for (int i = 0; i < accounts.Length; i++)
        {
            Debug.Log(accounts[i].username);
            playerData.Add(new PlayerData(accounts[i]));
        }

        inputShot.Initialize();
    }

    public void DecideTeams()
    {
        int goldScore = -1;
        int silverScore = -1;
        int bronzeScore = -1;

        Player gold = null;
        Player silver = null;
        Player bronze = null;
        List<Player> other = new List<Player>();

        for (int i = 0; i < playerData.Count; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, redParent);
            Player current = playerObj.GetComponent<Player>();

            current.playerName = playerData[i].account.username;
            current.playerColor =playerData[i].account.preferredColor;
            current.totalScore = playerScore[i];

            if (playerScore[i] > goldScore)
            {
                if (gold != null)
                {
                    if (silver != null)
                    {
                        bronze = silver;
                        bronzeScore = silverScore;
                    }
                    silver = gold;
                    silverScore = goldScore;
                }

                goldScore = playerScore[i];
                gold = current;
            }
            else if (playerScore[i] > silverScore)
            {
                if (silver != null)
                {
                    if (bronze != null)
                    {
                        other.Add(bronze);
                    }
                    bronze = silver;
                    bronzeScore = silverScore;
                }

                silverScore = playerScore[i];
                silver = current;
            }
            else if (playerScore[i] > bronzeScore)
            {
                if (bronze != null)
                {
                    other.Add(bronze);
                }

                bronzeScore = playerScore[i];
                bronze = current;
            }
            else
            {
                other.Add(current);
            }
        }

        gold.Initalize(gold.playerName, gold.totalScore, teamColors[0]);
        gold.PoduimFinish(poduimFinishColor[0]);

        if (playerData.Count > 1)
        {
            silver.Initalize(silver.playerName, silver.totalScore, teamColors[1]);
            silver.PoduimFinish(poduimFinishColor[1]);
        }

        if (playerData.Count > 2)
        {
            bronze.Initalize(bronze.playerName, bronze.totalScore, teamColors[1]);
            bronze.PoduimFinish(poduimFinishColor[2]);
        }

        if (playerData.Count > 3)
        {
            for (int i = 0; i < other.Count; i++)
            {
                if ((i + 1) % 2 == 0)
                {
                    other[i].Initalize(other[i].playerName, other[i].totalScore, teamColors[1]);
                }
                else
                {
                    other[i].Initalize(other[i].playerName, other[i].totalScore, teamColors[0]);
                }
            }
        }

        teamsScreen.SetActive(true);
        recap.StartRecap(clubs.ToArray());
    }
}
