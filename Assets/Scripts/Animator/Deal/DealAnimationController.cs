using UnityEngine;
using System.Collections;

public class DealAnimationController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private PlayerContext _player1;
    [SerializeField] private PlayerContext _player2;

    [Header("Animation")]
    [SerializeField] private CardTransitionManager _player1Transition;
    [SerializeField] private CardTransitionManager _player2Transition;
    [SerializeField] private CardFlipManager _flipManager;

    [Header("Game State")]
    [SerializeField] private GameplayStateManager _stateManager;

    public void PlayOpeningAnimation()
    {
        _stateManager.SetState(GameplayState.Busy);
        StartCoroutine(OpeningRoutine());
    }

    private IEnumerator OpeningRoutine()
    {
        _player1.HandDisplay.RefreshHand();
        _player2.HandDisplay.RefreshHand();
        _player1.HandDisplay.HideAllSlots();
        _player2.HandDisplay.HideAllSlots();

        for (int i = 0; i < HandManager.MaxHandSize; i++)
        {
            bool player1Finished = false;
            bool player2Finished = false;

            StartCoroutine(
                PlayDealAnimation(
                    _player1,
                    _player1Transition,
                    i,
                    () => player1Finished = true));

            StartCoroutine(
                PlayDealAnimation(
                    _player2,
                    _player2Transition,
                    i,
                    () => player2Finished = true));

            AudioManager.Instance.PlaySfx(GameSfx.CardFlip);
            yield return new WaitUntil(() => player1Finished && player2Finished);
            yield return new WaitForSeconds(0.05f);
        }
        
        _stateManager.SetState(GameplayState.Normal);
    }

    public IEnumerator PlayDealAnimation(
        PlayerContext player,
        CardTransitionManager transition,
        int slotIndex,
        System.Action onFinished)
    {
        bool finished = false;

        RectTransform from = player.SelectedCardDisplay.GetSlotTransform();
        RectTransform to = player.HandDisplay.GetSlotTransform(slotIndex);

        transition.PlaySingle(
            player.HandDisplay.ClosedCardSprite,
            from,
            to,
            () =>
            {
                finished = true;
            });

        yield return new WaitUntil(() => finished);

        if (player.PlayerSide == PlayerSide.Player1)
        {
            bool flipFinished = false;
            CardData card = player.HandManager.Hand[slotIndex];

            player.CardFlipManager.Play(
                card,
                player.HandDisplay.GetSlotTransform(slotIndex),
                () =>
                {
                    player.HandDisplay.ShowSlot(slotIndex);
                    flipFinished = true;
                });

            yield return new WaitUntil(() => flipFinished);
        }
        else
        {
            player.HandDisplay.ShowSlot(slotIndex);
        }

        onFinished?.Invoke();
    }
}