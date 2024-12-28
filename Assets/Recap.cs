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

        _clubIndex++;
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
        marker.GetComponent<Image>().color = Players.Instance.playerColor[playerIndex];
        marker.SetActive(true);

        GameObject line = Instantiate(linePrefab, parentHolder);

        CurvedLine curvedLine = line.GetComponent<CurvedLine>();
        curvedLine.endPoint = marker.transform.localPosition;
        curvedLine.color = Players.Instance.playerColor[playerIndex];
        curvedLine.enabled = true;

        _lines.Add(line);
        _markers.Add(marker);
    }
}
