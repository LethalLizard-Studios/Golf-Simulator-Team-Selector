using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ClubToggle : MonoBehaviour
{
    [SerializeField]
    private string displayName;

    [SerializeField]
    private Image image;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void CheckAndAdd()
    {
        if (!_toggle.isOn)
            return;

        Players players = Players.Instance;

        players.clubName.Add(displayName);
        players.clubImages.Add(image.sprite);
    }
}
