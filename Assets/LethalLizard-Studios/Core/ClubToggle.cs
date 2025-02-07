using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ClubToggle : MonoBehaviour
{
    [SerializeField] private string displayName;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image clubIconImage;

    private Toggle _toggle;
    private ClubSelection _clubSelection;

    public void Initialize(ClubSelection clubSelection)
    {
        _clubSelection = clubSelection;
        _toggle = GetComponent<Toggle>();
    }

    public string GetDisplayName()
    {
        return displayName;
    }

    public Sprite GetIcon()
    {
        return clubIconImage.sprite;
    }

    public void Pressed()
    {
        selectedImage.enabled = _toggle.isOn;

        if (_toggle.isOn)
        {
            _clubSelection.Toggle(this);
        }
        else
        {
            _clubSelection.Untoggle(this);
        }
    }
}
