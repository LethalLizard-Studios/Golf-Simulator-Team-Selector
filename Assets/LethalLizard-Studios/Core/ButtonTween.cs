using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(AudioSource))]
public class ButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip pressedClip;

    private AudioSource _audioSource;
    private RectTransform _rectTransform;
    private Image _image;
    private TextMeshProUGUI _text;
    private Button _button;

    private Color _originalColor;
    private Vector3 _originalScale;

    private const float HIGHLIGHT_TINT = 0.12f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rectTransform = GetComponent<RectTransform>();

        _originalScale = _rectTransform.localScale;

        if (TryGetComponent(out _button))
            _button.onClick.AddListener(() => OnClicked());

        if (TryGetComponent(out _image))
            _originalColor = _image.color;
        else if (TryGetComponent(out _text))
            _originalColor = _text.color;
    }

    private void OnDisable()
    {
        if (_image != null)
            _image.color = _originalColor;
        else if (_text != null)
            _text.color = _originalColor;

        _rectTransform.localScale = _originalScale;
    }

    public void OnClicked()
    {
        if (pressedClip != null)
        {
            _audioSource.clip = pressedClip;
            _audioSource.Play();
        }
        _rectTransform.DOPunchScale(_originalScale * -0.2f, 0.25f).SetUpdate(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null)
        {
            _audioSource.clip = hoverClip;
            _audioSource.Play();
        }
        _rectTransform.DOScale(_originalScale * 1.08f, 0.1f).SetUpdate(true);

        if (_image != null)
            _image.DOColor(_originalColor + (Color.white * HIGHLIGHT_TINT), 0.1f).SetUpdate(true);
        else if (_text != null)
            _text.DOColor(_originalColor + (Color.white * HIGHLIGHT_TINT), 0.1f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOScale(_originalScale, 0.3f).SetUpdate(true);

        if (_image != null)
            _image.DOColor(_originalColor, 0.3f).SetUpdate(true);
        else if (_text != null)
            _text.DOColor(_originalColor, 0.3f).SetUpdate(true);
    }
}
