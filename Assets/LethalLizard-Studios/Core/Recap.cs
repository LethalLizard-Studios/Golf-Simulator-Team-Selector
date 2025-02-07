using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Recap : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestClubName;

    [SerializeField]
    private TextMeshProUGUI bestPlayerClubName;

    [SerializeField]
    private Transform parentHolder;

    [SerializeField]
    private GameObject shotMarkerPrefab;

    [SerializeField]
    private GameObject linePrefab;

    private List<GameObject> _markers = new List<GameObject>();
    private List<GameObject> _lines = new List<GameObject>();

    private Club[] _currentClubs;
    private int _clubIndex = 0;

    public void StartRecap(Club[] clubs)
    {
        ClearAllShots();

        _clubIndex = 0;
        _currentClubs = clubs;

        StartCoroutine(ChangeClub());
    }

    private IEnumerator ChangeClub()
    {
        ClearAllShots();

        for (int i = 0; i < Players.Instance.numberOfPlayers; i++)
            SetupShot(i);

        bestClubName.text = "Best " + _currentClubs[_clubIndex].name + " Recap";
        bestPlayerClubName.text = _currentClubs[_clubIndex].bestPlayerName;

        yield return new WaitForSeconds(6.0f);

        if (_clubIndex < _currentClubs.Length - 1)
            _clubIndex++;
        else
            _clubIndex = 0;

        StartCoroutine(ChangeClub());
    }

    public void ClearAllShots()
    {
        for (int i = 0; i < _markers.Count; i++)
        {
            Destroy(_markers[i]);
            Destroy(_lines[i]);
        }
        _markers.Clear();
        _lines.Clear();
    }

    public void SetupShot(int playerIndex)
    {
        Players players = Players.Instance;

        GameObject marker = Instantiate(shotMarkerPrefab, parentHolder);
        marker.transform.localPosition = _currentClubs[_clubIndex].playerShots[playerIndex];
        marker.GetComponent<Image>().color = players.playerData[playerIndex].account.preferredColor;
        marker.SetActive(true);

        GameObject marker2 = Instantiate(shotMarkerPrefab, parentHolder);
        marker2.transform.localPosition = _currentClubs[_clubIndex].playerShotsSecondary[playerIndex];
        marker2.GetComponent<Image>().color = players.playerData[playerIndex].account.preferredColor;
        marker2.SetActive(true);

        GameObject line = Instantiate(linePrefab, parentHolder);

        CurvedLine curvedLine = line.GetComponent<CurvedLine>();
        curvedLine.endPoint = marker.transform.localPosition;
        curvedLine.color = players.playerData[playerIndex].account.preferredColor;
        curvedLine.controlPoint = new Vector2(0, curvedLine.endPoint.y * 0.8f);
        curvedLine.enabled = true;

        GameObject line2 = Instantiate(linePrefab, parentHolder);

        CurvedLine curvedLine2 = line2.GetComponent<CurvedLine>();
        curvedLine2.endPoint = marker2.transform.localPosition;
        curvedLine2.color = players.playerData[playerIndex].account.preferredColor;
        curvedLine2.controlPoint = new Vector2(0, curvedLine2.endPoint.y * 0.8f);
        curvedLine2.enabled = true;

        _lines.Add(line);
        _lines.Add(line2);
        _markers.Add(marker);
        _markers.Add(marker2);
    }
}
