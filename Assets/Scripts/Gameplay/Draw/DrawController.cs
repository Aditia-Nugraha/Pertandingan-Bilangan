using UnityEngine;

public class DrawController : MonoBehaviour
{
    [Header("PLayer")]
    [SerializeField] private PlayerContext _player;

    [Header("Game State")]
    [SerializeField] private GameplayStateManager _stateManager;

    [Header("Display")]
    [SerializeField] private SelectedCardDisplay _selectedCardDisplay;
    [SerializeField] private GameplayMessageDisplay _messageDisplay;

    [Header("Animation")]
    [SerializeField] private DealAnimationController _dealAnimationController;
    [SerializeField] private DrawAnimationService _drawAnimationService;
    [SerializeField] private CardTransitionManager _transitionManager;
    [SerializeField] private CardDestroyManager _destroyManager;

    private const int DrawCost = 5;

    public void OnDrawButton()
    {
        if (_stateManager.CurrentState == GameplayState.Busy)
        {
            return;
        }
        
        switch (_stateManager.CurrentState)
        {
            case GameplayState.Normal:
                Draw();
                break;

            case GameplayState.ReplaceCard:
                Discard();
                break;
        }
    }

    public void Draw()
    {
        int oldHP = _player.Status.HP;
        int oldEnergy = _player.Status.Energy;

        if (_player.Status.Energy < DrawCost)
        {
            AudioManager.Instance.PlaySfx(GameSfx.Error);
            _messageDisplay.Show(GameplayMessage.NotEnoughEnergy);
            return;
        }

        if (_player.HandManager.IsHandFull())
        {
            if (_player.HandManager.HasSelectedCard())
            {
                AudioManager.Instance.PlaySfx(GameSfx.CardMove);
                _player.HumanController.PlayReturnAnimation(() =>
                {
                    ContinueFullHandDraw(oldHP, oldEnergy);
                });

                return;
            }

            ContinueFullHandDraw(oldHP, oldEnergy);
            return;
        }

        if (_player.HandManager.HasSelectedCard())
        {
            AudioManager.Instance.PlaySfx(GameSfx.CardMove);
            _player.HumanController.PlayReturnAnimation(() =>
            {
                ContinueDraw(oldHP, oldEnergy);
            });

            return;
        }

        ContinueDraw(oldHP, oldEnergy);
        return;
    }

    private void ContinueDraw(int oldHP, int oldEnergy)
    {
        AudioManager.Instance.PlaySfx(GameSfx.CardFlip);
        _messageDisplay.Show(GameplayMessage.Draw);
        _player.Status.Energy -= DrawCost;
        int slotIndex = _player.HandManager.DrawOneCard();

        if (slotIndex < 0)
        {
            return;
        }

        if (_player.HandManager.HasSelectedCard())
        {
            _player.HumanController.PlayDrawAnimation(slotIndex, () =>
            {
                FinishDraw(oldHP, oldEnergy);
            });

            return;
        }

        StartCoroutine(
            _drawAnimationService.PlayDraw(
                _player,
                _transitionManager,
                slotIndex,
                () =>
                {
                    FinishDraw(oldHP, oldEnergy);
                }));
    }

    private void FinishDraw(int oldHP, int oldEnergy)
    {
        _player.HandDisplay.RefreshHand();
        _player.SelectedCardDisplay.Refresh();
        _player.StatusDisplay.AnimateRefresh(oldHP, oldEnergy);
    }

    private void ContinueFullHandDraw(int oldHP, int oldEnergy)
    {
        AudioManager.Instance.PlaySfx(GameSfx.CardFlip);
        _player.Status.Energy -= DrawCost;
        CardData drawnCard = _player.DeckManager.DrawOneCard();

        if (drawnCard == null)
        {
            return;
        }

        _player.TemporaryCard.SetCard(drawnCard);
        _player.CardFlipManager.Play(
            drawnCard,
            _player.SelectedCardDisplay.GetSlotTransform(),
            () =>
            {
                _player.SelectedCardDisplay.Refresh();
                _player.StatusDisplay.AnimateRefresh(oldHP, oldEnergy);
                _stateManager.SetState(GameplayState.ReplaceCard);
                _messageDisplay.Show(GameplayMessage.ReplaceCard);
            });
    }

    private void Discard()
    {
        if (!_player.TemporaryCard.HasCard)
        {
            return;
        }

        AudioManager.Instance.PlaySfx(GameSfx.Discard);
        _stateManager.SetState(GameplayState.Busy);
        CardData card = _player.TemporaryCard.Card;
        _player.SelectedCardDisplay.HideImage();

        _destroyManager.Play(
            card.CardSprite,
            _player.SelectedCardDisplay.GetSlotTransform(),
            () =>
            {
                _player.TemporaryCard.Clear();
                _player.SelectedCardDisplay.Refresh();

                _messageDisplay.Hide();
                _stateManager.SetState(GameplayState.Normal);
            });
    }
}
