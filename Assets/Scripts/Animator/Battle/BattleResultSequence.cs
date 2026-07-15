using UnityEngine;
using System.Collections;

public class BattleResultSequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleResultPanel _battleResultPanel;

    [Header("Animation")]
    [SerializeField] private ScalePanelAnimator _panelAnimator;
    [SerializeField] private BattleResultTextAnimator _roundAnimator;
    [SerializeField] private ResultPlayerDisplay _player1Display;
    [SerializeField] private ResultPlayerDisplay _player2Display;
    [SerializeField] private BattleResultTextAnimator _compareAnimator;
    [SerializeField] private BattleResultTextAnimator _battleResultAnimator;
    [SerializeField] private RewardAnimator _primaryRewardAnimator;
    [SerializeField] private RewardAnimator _energyRewardAnimator;
    [SerializeField] private ContinueButtonAnimator _continueButtonAnimator;

    [Header("Sequence")]
    [SerializeField] private float _phaseDelay = 0.3f;

    private Coroutine _sequenceCoroutine;

    public void Play()
    {
        if (_sequenceCoroutine != null)
        {
            StopCoroutine(_sequenceCoroutine);
        }

        _sequenceCoroutine = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        yield return PanelScale();
        yield return WaitPhase();

        yield return Round();
        yield return WaitPhase();

        yield return FlipCards();
        yield return WaitPhase();

        yield return CountValues();
        yield return WaitPhase();

        yield return Compare();
        yield return WaitPhase();

        yield return BattleResult();
        yield return WaitPhase();

        yield return PrimaryReward();
        yield return WaitPhase();

        yield return EnergyReward();
        yield return WaitPhase();

        yield return Continue();
        _sequenceCoroutine = null;
    }

    private IEnumerator WaitPhase()
    {
        yield return new WaitForSeconds(_phaseDelay);
    }

    private IEnumerator PanelScale()
    {
        bool finished = false;

        _panelAnimator.PlayShow(() =>
        {
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    private IEnumerator Round()
    {
        bool finished = false;

        _roundAnimator.Play(() =>
        {
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    private IEnumerator FlipCards()
    {
        bool player1Finished = false;
        bool player2Finished = false;

        _player1Display.PlayFlip(() =>
        {
            player1Finished = true;
        });

        _player2Display.PlayFlip(() =>
        {
            player2Finished = true;
        });

        yield return new WaitUntil(() =>
            player1Finished &&
            player2Finished);
    }

    private IEnumerator CountValues()
    {
        bool player1Finished = false;
        bool player2Finished = false;

        _player1Display.PlayValue(() =>
        {
            player1Finished = true;
        });

        _player2Display.PlayValue(() =>
        {
            player2Finished = true;
        });

        yield return new WaitUntil(() =>
            player1Finished &&
            player2Finished);
    }

    private IEnumerator Compare()
    {
        bool finished = false;

        _compareAnimator.Play(() =>
        {
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    private IEnumerator BattleResult()
    {
        bool finished = false;

        _battleResultAnimator.Play(() =>
        {
            finished = true;
        });

        AudioManager.Instance.PlaySfx(_battleResultPanel.GetBattleResultSfx());
        yield return new WaitUntil(() => finished);
    }

    private IEnumerator PrimaryReward()
    {
        bool finished = false;

        _primaryRewardAnimator.Play(() =>
        {
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    private IEnumerator EnergyReward()
    {
        bool finished = false;

        _energyRewardAnimator.Play(() =>
        {
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    private IEnumerator Continue()
    {
        bool finished = false;

        _continueButtonAnimator.Play(() =>
        {
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }
}