using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    public static Players Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Recap recap;
    [SerializeField] private GameObject teamsScreen;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform redParent;

    public GameObject numOfPlayersScreen;
    public int numberOfPlayers = 0;

    public List<string> playerName = new List<string>();
    public List<Color> playerColor = new List<Color>();
    public List<int> playerScore = new List<int>();

    public List<Color> availableColors = new List<Color>();

    [SerializeField] private Color[] poduimFinishColor = new Color[3];
    [SerializeField] private Color[] teamColors = new Color[2];

    [SerializeField] private Image colorPreview;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField playersInput;

    public InputShot shot;

    public string[] clubName;
    public Sprite[] clubImages;

    public List<Club> clubs = new List<Club>();

    private int currentColor = 0;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (playerName.Count < 1)
        {
            numOfPlayersScreen.SetActive(true);
        }
        else
        {
            numOfPlayersScreen.SetActive(false);
        }

        colorPreview.color = availableColors[0];
        currentColor = 0;

        nameInput.text = string.Empty;
    }

    public void ChangeColor()
    {
        if (currentColor + 1 < availableColors.Count)
            currentColor++;
        else
            currentColor = 0;

        colorPreview.color = availableColors[currentColor];
    }

    public void ConfirmPlayer()
    {
        if (numOfPlayersScreen.activeSelf)
        {
            numberOfPlayers = int.Parse(playersInput.text);

            for (int i = 0; i < clubName.Length; i++)
                clubs.Add(new Club(clubName[i], numberOfPlayers));
        }

        playerName.Add(nameInput.text);
        playerColor.Add(availableColors[currentColor]);

        availableColors.RemoveAt(currentColor);

        shot.PlayerAdded();

        Initialize();
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

        for (int i = 0; i < playerName.Count; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, redParent);
            Player current = playerObj.GetComponent<Player>();

            current.playerName = playerName[i];
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

        if (playerName.Count > 1)
        {
            silver.Initalize(silver.playerName, silver.totalScore, teamColors[1]);
            silver.PoduimFinish(poduimFinishColor[1]);
        }

        if (playerName.Count > 2)
        {
            bronze.Initalize(bronze.playerName, bronze.totalScore, teamColors[1]);
            bronze.PoduimFinish(poduimFinishColor[2]);
        }

        if (playerName.Count > 3)
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
