using System;
using System.Collections;
using UnityEngine;

public class ContinueButtonAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _rectTransform;

    [Header("Animation")]
    [SerializeField] private float _duration = 0.25f;

    private Vector3 _originalScale;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _originalScale = _rectTransform.localScale;
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
        _rectTransform.localScale = Vector3.zero;
        float timer = 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _duration);
            _rectTransform.localScale =
                Vector3.Lerp(
                    Vector3.zero,
                    _originalScale,
                    t);
            yield return null;
        }

        _rectTransform.localScale = _originalScale;
        _animationCoroutine = null;
        onFinished?.Invoke();
    }

    public void ResetState()
    {
        _rectTransform.localScale = Vector3.zero;
    }
}