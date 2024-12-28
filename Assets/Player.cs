using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Image medal;
    [SerializeField] private Image background;
    [SerializeField] private Image playerIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public int totalScore = 0;
    public string playerName = "";
    public Color playerColor;

    public void Initalize(string name, int score, Color team)
    {
        this.nameText.text = name;
        this.scoreText.text = ""+score;

        playerIcon.color = playerColor;
        background.color = team;
    }

    public void PoduimFinish(Color place)
    {
        medal.color = place;
    }
}
