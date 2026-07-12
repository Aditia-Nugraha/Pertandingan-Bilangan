using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ResultValueAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private RectTransform _rectTransform;

    [Header("Animation")]
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _moveDistance = 20f;

    private Vector2 _defaultPosition;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _defaultPosition = _rectTransform.anchoredPosition;
    }

    public void Play(float targetValue, Action onFinished = null)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(
            PlayRoutine(targetValue, onFinished));
    }

    private IEnumerator PlayRoutine(float targetValue, Action onFinished)
    {
        float timer = 0f;
        Color color = _valueText.color;
        color.a = 0f;
        _valueText.color = color;
        _rectTransform.anchoredPosition = _defaultPosition - Vector2.up * _moveDistance;

        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _duration);
            color.a = t;
            _valueText.color = color;
            _rectTransform.anchoredPosition =
                Vector2.Lerp(
                    _defaultPosition - Vector2.up * _moveDistance,
                    _defaultPosition,
                    t);
            float value = Mathf.Lerp(0f, targetValue, t);
            _valueText.text = value.ToString("0.000");
            yield return null;
        }

        _valueText.text = targetValue.ToString("0.000");
        color.a = 1f;
        _valueText.color = color;
        _rectTransform.anchoredPosition = _defaultPosition;
        _animationCoroutine = null;
        onFinished?.Invoke();
    }
}