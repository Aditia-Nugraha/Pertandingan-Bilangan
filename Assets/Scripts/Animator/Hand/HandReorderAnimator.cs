using UnityEngine;
using System;
using System.Collections;

public class HandReorderAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private RectTransform _rectTransform;

    [Header("Animation")]
    [SerializeField] private float _moveDuration = 0.15f;

    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _cardDisplay.ClearCard();
    }

    public void Play(
        Sprite sprite,
        RectTransform from,
        RectTransform to,
        Action onFinished = null)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine =
            StartCoroutine(
                PlayRoutine(
                    sprite,
                    from,
                    to,
                    onFinished));
    }

    private IEnumerator PlayRoutine(
        Sprite sprite,
        RectTransform from,
        RectTransform to,
        Action onFinished)
    {
        _rectTransform.position = from.position;
        _cardDisplay.SetSprite(sprite);
        float timer = 0f;

        while (timer < _moveDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _moveDuration);
            _rectTransform.position =
                Vector3.Lerp(
                    from.position,
                    to.position,
                    t);
            yield return null;
        }

        _rectTransform.position = to.position;
        _cardDisplay.ClearCard();
        _animationCoroutine = null;
        onFinished?.Invoke();
    }
}