using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicKits : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private TMP_Dropdown dropdown;

    [System.Serializable]
    public class Music
    {
        public string displayName;
        public AudioClip audioClip;
    }

    public Music[] music;

    private void Awake()
    {
        PopulateDropdown();
    }

    private void PopulateDropdown()
    {
        dropdown.ClearOptions();  // Clear existing options

        List<string> options = new List<string>();

        foreach (Music track in music)
        {
            options.Add(track.displayName);
        }
        dropdown.AddOptions(options);
    }

    public void SelectSong(int index)
    {
        audioSource.clip = music[index].audioClip;
        audioSource.Play();
    }
}
