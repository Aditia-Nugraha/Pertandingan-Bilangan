using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _valueText;

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
        Color iconColor = _icon.color;
        Color textColor = _valueText.color;
        iconColor.a = 0f;
        textColor.a = 0f;
        _icon.color = iconColor;
        _valueText.color = textColor;
        _rectTransform.anchoredPosition = _originalPosition - Vector2.up * _moveDistance;
        float timer = 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _duration);
            iconColor.a = t;
            textColor.a = t;
            _icon.color = iconColor;
            _valueText.color = textColor;
            _rectTransform.anchoredPosition =
                Vector2.Lerp(
                    _originalPosition - Vector2.up * _moveDistance,
                    _originalPosition,
                    t);
            yield return null;
        }

        iconColor.a = 1f;
        textColor.a = 1f;
        _icon.color = iconColor;
        _valueText.color = textColor;
        _rectTransform.anchoredPosition = _originalPosition;
        _animationCoroutine = null;
        onFinished?.Invoke();
    }
}