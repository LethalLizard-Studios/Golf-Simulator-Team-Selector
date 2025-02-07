using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Image rarityImage;
    [SerializeField] private Image iconImage;

    public void Initialize(string name, Sprite clubIcon, Color color)
    {
        displayNameText.text = name;
        iconImage.sprite = clubIcon;
        rarityImage.color = color;
    }
}
