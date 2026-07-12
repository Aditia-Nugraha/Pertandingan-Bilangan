using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BattleResultTextAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _text;
    [SerializeField] private RectTransform _rectTransform;

    [Header("Animation")]
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private float _moveDistance = 25f;

    private Vector2 _originalPosition;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _originalPosition = _rectTransform.anchoredPosition;
    }

    public void Play(Action onFinished = null)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(
            PlayRoutine(onFinished));
    }

    private IEnumerator PlayRoutine(Action onFinished)
    {
        _text.alpha = 0f;
        _rectTransform.anchoredPosition = _originalPosition - Vector2.up * _moveDistance;
        float timer = 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _duration);
            _text.alpha = Mathf.Lerp(0f, 1f, t);
            _rectTransform.anchoredPosition =
                Vector2.Lerp(
                    _originalPosition - Vector2.up * _moveDistance,
                    _originalPosition,
                    t);

            yield return null;
        }

        _text.alpha = 1f;
        _rectTransform.anchoredPosition = _originalPosition;
        _animationCoroutine = null;
        onFinished?.Invoke();
    }
}