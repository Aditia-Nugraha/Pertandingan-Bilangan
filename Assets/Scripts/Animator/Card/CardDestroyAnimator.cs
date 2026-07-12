using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CardDestroyAnimator : MonoBehaviour
{
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;
    [SerializeField] private float _duration = 0.2f;

    private Vector3 _defaultScale;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultScale = _rectTransform.localScale;
        _defaultColor = _image.color;
        _cardDisplay.ClearCard();
    }

    public void Play(Sprite sprite, RectTransform target, Action onFinished = null)
    {
        StopAllCoroutines();
        StartCoroutine(PlayRoutine(sprite, target, onFinished));
    }

    private IEnumerator PlayRoutine(Sprite sprite, RectTransform target, Action onFinished)
    {
        _cardDisplay.SetSprite(sprite);
        _rectTransform.position = target.position;
        _rectTransform.localScale = _defaultScale;
        _image.color = _defaultColor;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);
            _rectTransform.localScale = Vector3.Lerp(
                _defaultScale,
                Vector3.zero,
                t);

            Color color = _defaultColor;
            color.a = Mathf.Lerp(1f, 0f, t);
            _image.color = color;
            yield return null;
        }

        _cardDisplay.ClearCard();
        _rectTransform.localScale = _defaultScale;
        _image.color = _defaultColor;
        onFinished?.Invoke();
    }
}