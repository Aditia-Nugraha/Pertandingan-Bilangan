using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayMessageAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private RectTransform _rectTransform;

    [Header("Animation")]
    [SerializeField] private float _visibleDuration = 1f;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _moveDistance = 25f;

    private Coroutine _animationCoroutine;
    private Vector2 _originalPosition;

    private void Awake()
    {
        _originalPosition = _rectTransform.anchoredPosition;
    }

    public void Play()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        _messageText.alpha = 1f;
        _rectTransform.anchoredPosition = _originalPosition;
        yield return new WaitForSeconds(_visibleDuration);
        yield return FadeOutRoutine();
    }

    public void PlayHide()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _animationCoroutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return FadeOutRoutine();
    }

    private IEnumerator FadeOutRoutine()
    {
        float timer = 0f;

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _fadeDuration;
            _messageText.alpha = Mathf.Lerp(1f, 0f, t);

            _rectTransform.anchoredPosition =
                Vector2.Lerp(
                    _originalPosition,
                    _originalPosition + Vector2.down * _moveDistance,
                    t);

            yield return null;
        }

        _messageText.alpha = 0f;
        _messageText.text = "";
        _rectTransform.anchoredPosition = _originalPosition;
        _animationCoroutine = null;
    }
}