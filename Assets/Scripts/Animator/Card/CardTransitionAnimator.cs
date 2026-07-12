using UnityEngine;
using System;
using System.Collections;

public class CardTransitionAnimator : MonoBehaviour
{
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _duration = 0.3f;

    private void Awake()
    {
        _cardDisplay.ClearCard();
    }

    public void Play(Sprite sprite, RectTransform from, RectTransform to, Action onFinished = null)
    {
        StopAllCoroutines();
        StartCoroutine(PlayRoutine(sprite, from, to, onFinished));
    }

    private IEnumerator PlayRoutine(Sprite sprite, RectTransform from, RectTransform to, Action onFinished)
    {
        _cardDisplay.SetSprite(sprite);
        _rectTransform.position = from.position;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);
            t = Mathf.SmoothStep(0f, 1f, t);
            _rectTransform.position = Vector3.Lerp(from.position, to.position, t);
            yield return null;
        }

        _rectTransform.position = to.position;
        _cardDisplay.ClearCard();
        onFinished?.Invoke();
    }
}