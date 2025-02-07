using DG.Tweening;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private GameObject spinButton;
    [SerializeField] private RectTransform spread;
    [SerializeField] private Transform list;
    [SerializeField] private AudioSource tickSound;
    [SerializeField] private WheelSpot[] wheelSpots;

    [System.Serializable]
    public class SpinItem
    {
        public string displayName;
        public Sprite icon;
        public WheelRarity rarity = WheelRarity.Common;
    }

    [SerializeField] private SpinItem[] spinItems;

    private float _targetX;
    private float _lastBumpX;
    private float _originalHeight;

    private const int MIN_MOVE = 6500;
    private const int MAX_MOVE = 7500;
    private const float DURATION = 5.0f;

    private const int BUMP_INTERVAL = 180;
    private const int BUMP_HEIGHT = 25;
    private const float BUMP_DURATION = 0.2f;


    private void Awake()
    {
        _originalHeight = spread.sizeDelta.y;
    }

    public void ClickSpin()
    {
        list.localPosition = new Vector3(-478.0f, 0f, 0f);
        spinButton.SetActive(false);

        for (int i = 0; i < wheelSpots.Length; i++)
        {
            SpinItem randomItem = spinItems[Random.Range(0, spinItems.Length)];
            wheelSpots[i].Initialize(randomItem.displayName, randomItem.icon, randomItem.rarity.GetColor());
        }

        StartSpin();
    }

    private void StartSpin()
    {
        _targetX = list.position.x - Random.Range(MIN_MOVE, MAX_MOVE);
        _lastBumpX = list.localPosition.x;

        list.DOMoveX(_targetX, DURATION)
            .SetEase(Ease.OutQuad)
            .OnUpdate(CheckForBump)
            .OnComplete(() =>
                spinButton.SetActive(true));
    }

    private void CheckForBump()
    {
        float movedX = _lastBumpX - list.localPosition.x;

        if (movedX >= BUMP_INTERVAL)
        {
            _lastBumpX = list.localPosition.x;
            DoBump();
        }
    }

    private void DoBump()
    {
        tickSound.Play();

        float newHeight = _originalHeight + BUMP_HEIGHT;

        DOTween.To(() => spread.sizeDelta, x => spread.sizeDelta = x, new Vector2(spread.sizeDelta.x, newHeight), BUMP_DURATION / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
                DOTween.To(() => spread.sizeDelta, x => spread.sizeDelta = x, new Vector2(spread.sizeDelta.x, _originalHeight), BUMP_DURATION)
                .SetEase(Ease.InQuad));
    }
}
