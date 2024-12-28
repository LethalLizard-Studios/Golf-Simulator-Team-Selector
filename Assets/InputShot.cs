using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class InputShot : MonoBehaviour
{
    public int numberOfShots = 3;

    [SerializeField] private GameObject shotMarkerPrefab;
    [SerializeField] private GameObject connectorPrefab;
    [SerializeField] private Transform shotParent;

    [SerializeField] private GameObject shotScreen;
    [SerializeField] private GameObject playerScreen;

    [SerializeField] private Transform carryMarker;

    [SerializeField] private TextMeshProUGUI playerNameTxt;

    [SerializeField] private TMP_InputField carryInput;
    [SerializeField] private TMP_InputField offlineInput;

    [SerializeField] private TextMeshProUGUI shotNumTxt;
    [SerializeField] private TextMeshProUGUI clubNameTxt;
    [SerializeField] private Image clubImage;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    [SerializeField] private int carryDist = 0;
    [SerializeField] private float offline = 0.0f;

    [SerializeField] private int result = 0;

    private const float OFFLINE_WEIGHT = 0.045f;

    // 0 = CARRY, 1 = OFFLINE WHOLE, 2 = OFFLINE DECIMAL, 3 = NEXT SHOT/PERSON
    private int state = 0;

    private int currentShot = 1;
    private int currentPlayer = 0;
    private int currentClub = 0;

    private List<GameObject> displayShots = new List<GameObject>();

    private void Start()
    {
        carryMarker.gameObject.SetActive(false);

        AddPlayer();

        clubNameTxt.text = Players.Instance.clubName[0];
        clubImage.sprite = Players.Instance.clubImages[0];

        shotNumTxt.text = currentShot + " / " + numberOfShots;
    }

    private void AddPlayer()
    {
        shotScreen.SetActive(false);
        playerScreen.SetActive(true);
    }

    public void PlayerAdded()
    {
        playerNameTxt.text = Players.Instance.playerName[currentPlayer];

        shotScreen.SetActive(true);
        playerScreen.SetActive(false);
    }

    private List<Vector3> lastPos = new List<Vector3>();
    private List<int> lastScore = new List<int>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightShift))
        {
            switch (state)
            {
                case 0:
                    carryMarker.gameObject.SetActive(false);

                    if (carryInput.text != string.Empty)
                        carryDist = int.Parse(carryInput.text);
                    else
                        carryDist = 0;

                    if (offlineInput.text != string.Empty)
                        offline = float.Parse(offlineInput.text);
                    else
                        offline = 0;

                    if (carryDist == 0)
                        return;

                    result = CalcShotScore(carryDist, offline);
                    scoreTxt.text = ""+result;

                    lastScore.Add(result);
                    lastPos.Add(new Vector3(Mathf.Clamp(offline * 6f, -160f, 160f), carryDist, 0f));

                    Vector3 markerPos = new Vector3(Mathf.Clamp(offline * 6f, -160f, 160f), carryDist, 0f);
                    carryMarker.localPosition = markerPos;
                    carryMarker.gameObject.SetActive(true);

                    Club club = Players.Instance.clubs[currentClub];
                    club.playerShots[currentPlayer] = new Vector2(markerPos.x, markerPos.y);

                    if (result > club.bestPlayerResult)
                    {
                        club.bestPlayerResult = result;
                        club.bestPlayerName = Players.Instance.playerName[currentPlayer];
                    }

                    break;
                case 1:
                    GameObject shotMarker = Instantiate(shotMarkerPrefab, shotParent);
                    displayShots.Add(shotMarker);

                    shotMarker.GetComponent<Image>().color = Players.Instance.playerColor[currentPlayer];
                    shotMarker.transform.localPosition = new Vector3(Mathf.Clamp(offline * 6f, -160f, 160f), carryDist, 0f);

                    carryMarker.gameObject.SetActive(false);

                    GameObject line = Instantiate(connectorPrefab, shotParent);
                    
                    CurvedLine curvedLine = line.GetComponent<CurvedLine>();
                    curvedLine.endPoint = shotMarker.transform.localPosition;
                    curvedLine.color = Players.Instance.playerColor[currentPlayer];
                    curvedLine.enabled = true;

                    displayShots.Add(line);

                    currentShot++;

                    carryInput.text = string.Empty;
                    offlineInput.text = string.Empty;

                    if (currentShot > numberOfShots)
                    {
                        int clubScore = FinalClubScore(lastPos.ToArray(), lastScore.ToArray());

                        if (Players.Instance.playerScore.Count == currentPlayer)
                        {
                            Players.Instance.playerScore.Add(clubScore);
                        }
                        else
                        {
                            Players.Instance.playerScore[currentPlayer] += clubScore;
                        }

                        Debug.Log(clubScore);

                        lastPos.Clear();
                        lastScore.Clear();

                        scoreTxt.text = "0";

                        currentShot = 1;
                        currentPlayer++;

                        if (currentPlayer < Players.Instance.numberOfPlayers)
                        {
                            if (currentClub == 0)
                            {
                                AddPlayer();
                            }
                            else
                            {
                                playerNameTxt.text = Players.Instance.playerName[currentPlayer];
                                playerNameTxt.transform.DOScale(1.3f, 0.5f / 2)
                                    .SetEase(Ease.OutBack)
                                    .OnComplete(() =>
                                    {
                                        playerNameTxt.transform.DOScale(1f, 0.5f / 2).SetEase(Ease.InBack);
                                    });
                            }
                        }
                        else
                        {
                            if (currentClub + 1 < Players.Instance.clubName.Length)
                            {
                                foreach (GameObject marker in displayShots)
                                {
                                    Destroy(marker);
                                }

                                currentClub++;
                                clubNameTxt.text = Players.Instance.clubName[currentClub];
                                clubImage.sprite = Players.Instance.clubImages[currentClub];
                                clubImage.transform.DOScale(1.3f, 0.5f / 2)
                                    .SetEase(Ease.OutBack)
                                    .OnComplete(() =>
                                    {
                                        clubImage.transform.DOScale(1f, 0.5f / 2).SetEase(Ease.InBack);
                                    });

                                currentPlayer = 0;
                                playerNameTxt.text = Players.Instance.playerName[currentPlayer];
                                playerNameTxt.transform.DOScale(1.3f, 0.5f / 2)
                                    .SetEase(Ease.OutBack)
                                    .OnComplete(() =>
                                    {
                                        playerNameTxt.transform.DOScale(1f, 0.5f / 2).SetEase(Ease.InBack);
                                    });

                                currentShot = 1;
                            }
                            else
                            {
                                Players.Instance.DecideTeams();

                                shotScreen.SetActive(false);
                                this.enabled = false;
                                return;
                            }
                        }
                    }

                    shotNumTxt.text = currentShot + " / " + numberOfShots;

                    state = 0;
                    return;
            }

            state = Mathf.Clamp(state + 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            state = Mathf.Clamp(state - 1, 0, 1);
            carryMarker.gameObject.SetActive(false);
        }
    }

    public int CalcShotScore(int carryDist, float offline)
    {
        int score = Mathf.RoundToInt(carryDist * Mathf.Clamp(((1 / (1 + Mathf.Abs(offline))) * OFFLINE_WEIGHT), 0.75f, 1f));

        return score;
    }

    public int FinalClubScore(Vector3[] position, int[] scores)
    {
        int scoreAvg = Mathf.RoundToInt((scores[0] + scores[1]) / 2f);

        //Consistency Bonus
        if (Vector3.Distance(position[0], position[1]) < 10)
        {
            scoreAvg = Mathf.RoundToInt(scoreAvg * 1.1f);
        }

        return scoreAvg;
    }
}
