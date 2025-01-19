using System.Collections.Generic;
using UnityEngine;

public class ClubSelection : MonoBehaviour
{
    [SerializeField] private ClubToggle[] clubToggles;

    [SerializeField] private GameObject errorMessage;
    [SerializeField] private GameObject confirmButton;

    private List<ClubToggle> _toggledClubs = new List<ClubToggle>();

    private const int CLUBS_REQUIRED = 2;

    private void Awake()
    {
        for (int i = 0; i < clubToggles.Length; i++)
        {
            clubToggles[i].Initialize(this);
        }
    }

    public ClubToggle GetClub(int index)
    {
        return _toggledClubs[index];
    }

    public int GetClubCount()
    {
        return _toggledClubs.Count;
    }

    public void Toggle(ClubToggle toggle)
    {
        _toggledClubs.Add(toggle);
        Refresh();
    }

    public void Untoggle(ClubToggle toggle)
    {
        if (!_toggledClubs.Contains(toggle))
        {
            return;
        }

        _toggledClubs.Remove(toggle);
        Refresh();
    }

    private void Refresh()
    {
        errorMessage.SetActive(_toggledClubs.Count < CLUBS_REQUIRED);
        confirmButton.SetActive(!errorMessage.activeSelf);
    }
}
