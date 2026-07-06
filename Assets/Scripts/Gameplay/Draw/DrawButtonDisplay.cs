using UnityEngine;
using UnityEngine.UI;

public class DrawButtonDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameplayStateManager _stateManager;
    [SerializeField] private Image _buttonImage;

    [Header("Sprites")]
    [SerializeField] private Sprite _drawSprite;
    [SerializeField] private Sprite _discardSprite;

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        _stateManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        _stateManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameplayState state)
    {
        Refresh();
    }

    private void Refresh()
    {
        _buttonImage.sprite = _stateManager.IsState(GameplayState.ReplaceCard)
            ? _discardSprite
            : _drawSprite;
    }
}