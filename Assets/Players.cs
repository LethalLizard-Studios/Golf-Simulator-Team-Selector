using System.Collections.Generic;
using System.Linq;
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

    public List<PlayerData> playerData = new List<PlayerData>();

    public List<int> playerScore = new List<int>();

    public Color[] colors;

    private Dictionary<int, Color> _availableColors = new Dictionary<int, Color>();

    [SerializeField] private Color[] poduimFinishColor = new Color[3];
    [SerializeField] private Color[] teamColors = new Color[2];

    [SerializeField] private Image colorPreview;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField playersInput;
    [SerializeField] private TMP_Dropdown musicDropdown;

    public InputShot shot;

    public string[] clubName;
    public Sprite[] clubImages;

    public List<Club> clubs = new List<Club>();

    private int currentColor = 0;
    private int currentColorIndex = 0;

    private void Start()
    {
        _availableColors.Clear();
        for (int i = 0; i < colors.Length; i++)
            _availableColors.Add(i, colors[i]);

        Initialize();
    }

    public void Initialize()
    {
        if (playerData.Count < 1)
        {
            numOfPlayersScreen.SetActive(true);
        }
        else
        {
            numOfPlayersScreen.SetActive(false);
        }

        colorPreview.color = _availableColors.Values.ToArray()[0];
        currentColor = 0;
        currentColorIndex = _availableColors.Keys.ToArray()[0];

        nameInput.text = string.Empty;
    }

    public void ChangeColor()
    {
        if (currentColor + 1 < _availableColors.Count)
            currentColor++;
        else
            currentColor = 0;

        colorPreview.color = _availableColors.Values.ToArray()[currentColor];
        currentColorIndex = _availableColors.Keys.ToArray()[currentColor];
    }

    public void ConfirmPlayer()
    {
        if (playersInput.text.Length <= 0 || int.Parse(playersInput.text) == 0)
            return;

        if (numOfPlayersScreen.activeSelf)
        {
            numberOfPlayers = int.Parse(playersInput.text);

            for (int i = 0; i < clubName.Length; i++)
                clubs.Add(new Club(clubName[i], numberOfPlayers));
        }

        playerData.Add(new PlayerData(nameInput.text, currentColorIndex, musicDropdown.value));
        _availableColors.Remove(currentColorIndex);

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

        for (int i = 0; i < playerData.Count; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, redParent);
            Player current = playerObj.GetComponent<Player>();

            current.playerName = playerData[i].displayName;
            current.playerColor =colors[playerData[i].colorIndex];
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
