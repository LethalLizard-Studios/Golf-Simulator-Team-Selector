using TMPro;
using UnityEngine;

public class MusicKits : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private TextMeshProUGUI trackText;
    [SerializeField] private TextMeshProUGUI artistText;

    [System.Serializable]
    public class Music
    {
        public Music(string artistName, string trackName, AudioClip audio, string modPath)
        {
            this.artistName = artistName;
            this.trackName = trackName;
            audioClip = audio;
            _modPath = modPath;
        }

        public string trackName;
        public string artistName;
        public AudioClip audioClip;

        private string _modPath = "";

        public string GetModPath()
        {
            return _modPath;
        }
    }

    public Music[] music;

    private int _currentSongIndex = 0;

    private void OnEnable()
    {
        _currentSongIndex = 0;
        RefreshInterface();
    }

    public string GetMusicName()
    {
        return music[_currentSongIndex].GetModPath() + music[_currentSongIndex].trackName;
    }

    public void Next()
    {
        _currentSongIndex = (_currentSongIndex + 1) % music.Length;
        RefreshInterface();
    }

    public void Previous()
    {
        _currentSongIndex = ((_currentSongIndex - 1) < 0) ? music.Length - 1 : _currentSongIndex - 1;
        RefreshInterface();
    }

    private void RefreshInterface()
    {
        if (_currentSongIndex >= music.Length && _currentSongIndex < 0)
            return;

        trackText.text = music[_currentSongIndex].trackName;
        artistText.text = music[_currentSongIndex].artistName;
        audioSource.Stop();

        if (music[_currentSongIndex].audioClip != null)
        {
            audioSource.clip = music[_currentSongIndex].audioClip;
            audioSource.Play();
        }
    }
}
