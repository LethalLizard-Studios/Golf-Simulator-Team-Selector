using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserActive : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private Image preferredColorImage;

    public void Initialize(string username, Color color)
    {
        usernameText.text = username;
        preferredColorImage.color = color;
    }
}
