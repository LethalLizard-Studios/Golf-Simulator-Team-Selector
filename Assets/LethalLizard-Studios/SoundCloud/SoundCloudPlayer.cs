using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SoundCloudPlayer : MonoBehaviour
{
    private string clientID = "YOUR_CLIENT_ID";  // Replace with your SoundCloud API key
    private string trackURL = "https://soundcloud.com/some-artist/song-title";
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        //StartCoroutine(GetTrackStreamURL(trackURL));
    }

    IEnumerator GetTrackStreamURL(string url)
    {
        // Convert SoundCloud track URL to API request
        string apiURL = $"https://api.soundcloud.com/resolve.json?url={url}&client_id={clientID}";

        using (UnityWebRequest request = UnityWebRequest.Get(apiURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response
                string json = request.downloadHandler.text;
                SoundCloudTrack track = JsonUtility.FromJson<SoundCloudTrack>(json);

                if (track.streamable)
                {
                    string streamURL = track.stream_url + $"?client_id={clientID}";
                    StartCoroutine(PlayAudioFromURL(streamURL));
                }
                else
                {
                    Debug.LogError("Track is not streamable.");
                }
            }
            else
            {
                Debug.LogError("Error fetching track: " + request.error);
            }
        }
    }

    IEnumerator PlayAudioFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Error loading audio: " + request.error);
            }
        }
    }
}

[System.Serializable]
public class SoundCloudTrack
{
    public string stream_url;
    public bool streamable;
}
