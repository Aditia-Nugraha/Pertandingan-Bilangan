using UnityEngine;
using System;
using System.Collections;

public class ScalePanelAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _panel;

    [Header("Animation")]
    [SerializeField] private float _showDuration = 0.3f;
    [SerializeField] private float _hideDuration = 0.25f;

    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _panel.localScale = Vector3.zero;
    }

    public void PlayShow(Action onFinished = null)
    {
        Play(Vector3.zero, Vector3.one, _showDuration, onFinished);
    }

    public void PlayHide(Action onFinished = null)
    {
        Play(Vector3.one, Vector3.zero, _hideDuration, onFinished);
    }

    private void Play(Vector3 from, Vector3 to, float duration, Action onFinished)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(
            PlayRoutine(from, to, duration, onFinished));
    }

    private IEnumerator PlayRoutine(
        Vector3 from,
        Vector3 to,
        float duration,
        Action onFinished)
    {
        float timer = 0f;
        _panel.localScale = from;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / duration);
            _panel.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }

        _panel.localScale = to;
        _animationCoroutine = null;
        onFinished?.Invoke();
    }
}