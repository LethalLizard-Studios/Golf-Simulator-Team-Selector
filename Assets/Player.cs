using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Image medal;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI score;

    public int totalScore = 0;
    public string playerName = "";

    public void Initalize(string name, int score, Color team)
    {
        this.name.text = name;
        this.score.text = ""+score;

        background.color = team;
    }

    public void PoduimFinish(Color place)
    {
        medal.color = place;
    }
}
