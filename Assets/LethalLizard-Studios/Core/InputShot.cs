using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputShot : MonoBehaviour
{
    public int numberOfShots = 3;

    [SerializeField] private MusicKits musicKits;

    [SerializeField] private GameObject shotMarkerPrefab;
    [SerializeField] private GameObject connectorPrefab;
    [SerializeField] private Transform shotParent;

    [SerializeField] private GameObject shotScreen;

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

    [SerializeField] private ClubSelection clubSelection;

    private const float OFFLINE_WEIGHT = 0.045f;

    // 0 = CARRY, 1 = OFFLINE WHOLE, 2 = OFFLINE DECIMAL, 3 = NEXT SHOT/PERSON
    private int state = 0;

    private int currentShot = 1;
    private int currentPlayer = 0;
    private int _currentClub = 0;

    private List<GameObject> displayShots = new List<GameObject>();

    private List<Vector3> lastPos = new List<Vector3>();
    private List<int> lastScore = new List<int>();

    private bool isInitialized = false;

    public void Initialize()
    {
        carryMarker.gameObject.SetActive(false);

        _currentClub = 0;
        RefreshClubDisplay();

        shotNumTxt.text = currentShot + " / " + numberOfShots;

        ChangeTargetPlayer();

        isInitialized = true;
    }

    private void RefreshClubDisplay()
    {
        clubNameTxt.text = clubSelection.GetClub(_currentClub).GetDisplayName();
        clubImage.sprite = clubSelection.GetClub(_currentClub).GetIcon();
        clubImage.transform.DOScale(1.3f, 0.5f / 2)
                           .SetEase(Ease.OutBack)
                           .OnComplete(() =>
                           {
                               clubImage.transform.DOScale(1f, 0.5f / 2).SetEase(Ease.InBack);
                           });
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            carryInput.text = Random.Range(10, 300).ToString();
            offlineInput.text = Random.Range(-30.0f, 30.0f).ToString();
        }

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

                    Club club = Players.Instance.clubs[_currentClub];

                    if (club.playerShots[currentPlayer] == null || club.playerShots[currentPlayer] == Vector2.zero)
                        club.playerShots[currentPlayer] = new Vector2(markerPos.x, markerPos.y);
                    else
                        club.playerShotsSecondary[currentPlayer] = new Vector2(markerPos.x, markerPos.y);

                    if (result > club.bestPlayerResult)
                    {
                        club.bestPlayerResult = result;
                        club.bestPlayerName = Players.Instance.playerData[currentPlayer].account.username;
                    }

                    break;
                case 1:
                    GameObject shotMarker = Instantiate(shotMarkerPrefab, shotParent);
                    displayShots.Add(shotMarker);

                    shotMarker.GetComponent<Image>().color = Players.Instance.playerData[currentPlayer].account.preferredColor;
                    shotMarker.transform.localPosition = new Vector3(Mathf.Clamp(offline * 6f, -160f, 160f), carryDist, 0f);

                    carryMarker.gameObject.SetActive(false);

                    GameObject line = Instantiate(connectorPrefab, shotParent);
                    
                    CurvedLine curvedLine = line.GetComponent<CurvedLine>();
                    curvedLine.endPoint = shotMarker.transform.localPosition;
                    curvedLine.color = Players.Instance.playerData[currentPlayer].account.preferredColor;
                    curvedLine.controlPoint = new Vector2(0, curvedLine.endPoint.y * 0.8f);
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

                        lastPos.Clear();
                        lastScore.Clear();

                        scoreTxt.text = "0";

                        currentShot = 1;
                        currentPlayer++;

                        if (currentPlayer < Players.Instance.numberOfPlayers)
                        {
                            ChangeTargetPlayer();
                        }
                        else
                        {
                            if (_currentClub + 1 < clubSelection.GetClubCount())
                            {
                                foreach (GameObject marker in displayShots)
                                {
                                    Destroy(marker);
                                }

                                _currentClub++;
                                RefreshClubDisplay();

                                currentPlayer = 0;
                                ChangeTargetPlayer();

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

    private void ChangeTargetPlayer()
    {
        playerNameTxt.text = Players.Instance.playerData[currentPlayer].account.username;
        playerNameTxt.transform.DOScale(1.3f, 0.5f / 2)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                playerNameTxt.transform.DOScale(1f, 0.5f / 2).SetEase(Ease.InBack);
            });
    }

    public int CalcShotScore(int carryDist, float offline)
    {
        Account account = Players.Instance.playerData[currentPlayer].account;
        LiveTip.SpeakTip(account.username, account.isRightHanded, offline, carryDist);

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
