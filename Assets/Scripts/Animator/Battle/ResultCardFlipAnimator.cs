using System;
using System.Collections;
using UnityEngine;

public class ResultCardFlipAnimator : MonoBehaviour
{
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Sprite _closedCardSprite;
    [SerializeField] private float _duration = 0.2f;
    private Vector3 _defaultScale;

    private void Awake()
    {
        _defaultScale = _rectTransform.localScale;
        _cardDisplay.ClearCard();
    }

    public void ShowCardBack()
    {
        _cardDisplay.SetSprite(_closedCardSprite);
    }

    public void Play(CardData card, Action onFinished = null)
    {
        StopAllCoroutines();
        StartCoroutine(FlipRoutine(card, onFinished));
    }

    private IEnumerator FlipRoutine(CardData card, Action onFinished)
    {
        _rectTransform.localScale = _defaultScale;
        float halfDuration = _duration * 0.5f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            _rectTransform.localScale = new Vector3(
                Mathf.Lerp(_defaultScale.x, 0f, t),
                _defaultScale.y,
                _defaultScale.z);
            yield return null;
        }

        _cardDisplay.SetCard(card);
        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            _rectTransform.localScale = new Vector3(
                Mathf.Lerp(0f, _defaultScale.x, t),
                _defaultScale.y,
                _defaultScale.z);
            yield return null;
        }

        _rectTransform.localScale = _defaultScale;
        onFinished?.Invoke();
    }
}