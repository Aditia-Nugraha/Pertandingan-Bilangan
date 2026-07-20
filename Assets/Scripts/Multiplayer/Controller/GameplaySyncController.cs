using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySyncController : MonoBehaviour
{
    [Header("Card Database")]
    [SerializeField] private CardDatabase _cardDatabase;

    [Header("Players")]
    [SerializeField] private PlayerContext _localPlayer;
    [SerializeField] private PlayerContext _opponentPlayer;

    [Header("Display")]
    [SerializeField] private HandDisplay _opponentHandDisplay;
    [SerializeField] private SelectedCardDisplay _opponentSelectedCard;
    [SerializeField] private Sprite _closedCardSprite;

    [Header("Battle")]
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private BattleResultPanel _battleResultPanel;
    [SerializeField] private GameResultPanel _gameResultPanel;

    [Header("Message")]
    [SerializeField] private GameplayMessageDisplay _opponentMessageDisplay;
    [SerializeField] private DisconnectPopup _disconnectPopup;

    [Header("Animation")]
    [SerializeField] private CardTransitionManager _transitionManager;
    [SerializeField] private DrawAnimationService _drawAnimationService;
    [SerializeField] private CardDestroyManager _destroyManager;

    private bool _ignoreDisconnect;

    private void OnEnable()
    {
        NetworkManager.Instance.PacketReceived += HandlePacketReceived;
        _localPlayer.HandManager.OnCardSelected += HandleLocalCardSelected;
        NetworkManager.Instance.Disconnected += HandleNetworkDisconnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Instance == null)
        {
            return;
        }

        NetworkManager.Instance.PacketReceived -= HandlePacketReceived;
        _localPlayer.HandManager.OnCardSelected -= HandleLocalCardSelected;
        NetworkManager.Instance.Disconnected -= HandleNetworkDisconnected;
    }

    public void IgnoreNextDisconnect()
    {
        _ignoreDisconnect = true;
    }

    private void HandleNetworkDisconnected()
    {
        if (_ignoreDisconnect)
        {
            _ignoreDisconnect = false;
            return;
        }

        _disconnectPopup.Show();
    }

    private void HandlePacketReceived(NetworkPacket packet)
    {
        switch (packet.Command)
        {
            case NetworkCommand.SelectCard:
                HandleSelectCard(packet);
                break;

            case NetworkCommand.ReturnCard:
                PlayOpponentReturnAnimation();
                break;

            case NetworkCommand.DrawCard:
                PlayOpponentDraw(packet);
                break;

            case NetworkCommand.ReplaceCard:
                PlayOpponentReplace(packet);
                break;

            case NetworkCommand.Heal:
                PlayOpponentHeal(packet);
                break;

            case NetworkCommand.BattleStart:
                HandleBattleStart();
                break;

            case NetworkCommand.ContinueRound:
                HandleContinueRound();
                break;

            case NetworkCommand.UpdateStatus:
                PlayOpponentStatus(packet);
                break;

            case NetworkCommand.Rematch:
                HandleRematch();
                break;

            case NetworkCommand.Exit:
                HandleExit();
                break;

            case NetworkCommand.Disconnect:
                HandleDisconnect();
                break;
        }
    }

    private CardData GetCardById(int cardId)
    {
        foreach (CardData card in _cardDatabase.Cards)
        {
            if (card.CardId == cardId)
            {
                return card;
            }
        }

        return null;
    }

    private void HandleSelectCard(NetworkPacket packet)
    {
        string[] split = packet.Data.Split('|');
        int slotIndex = int.Parse(split[0]);
        int cardId = int.Parse(split[1]);
        CardData card = GetCardById(cardId);

        if (_opponentPlayer.HandManager.HasSelectedCard())
        {
            PlayOpponentReplaceAnimation(slotIndex, card);
        }
        else
        {
            PlayOpponentSelectAnimation(slotIndex, card);
        }
    }

    private void HandleLocalCardSelected(int slotIndex)
    {
        CardData card = _localPlayer.HandManager.SelectedCard.Card;
        string data = $"{slotIndex}|{card.CardId}";
        NetworkManager.Instance.Send(NetworkCommand.SelectCard, data);
    }

    private void HandleBattleStart()
    {
        _battleManager.StartBattle();
    }
    
    private void HandleContinueRound()
    {
        _battleResultPanel.Continue();
    }

    private void HandleRematch()
    {
        _gameResultPanel.Rematch();
    }

    private void HandleExit()
    {
        ExitGame();
    }

    private void HandleDisconnect()
    {
        _disconnectPopup.Show();
    }

    public void SendReturnCard()
    {
        NetworkManager.Instance.Send(NetworkCommand.ReturnCard);
    }

    public void SendDrawCard(int slotIndex, int cardId)
    {
        NetworkManager.Instance.Send(NetworkCommand.DrawCard, $"{slotIndex}|{cardId}");
    }

    public void SendReplaceCard(int slotIndex, int cardId)
    {
        NetworkManager.Instance.Send(NetworkCommand.ReplaceCard, $"{slotIndex}|{cardId}");
    }

    public void SendStatus()
    {
        NetworkManager.Instance.Send(NetworkCommand.UpdateStatus,
            $"{_localPlayer.Status.HP}|{_localPlayer.Status.Energy}");
    }

    public void SendBattleStart()
    {
        NetworkManager.Instance.Send(NetworkCommand.BattleStart);
        _battleManager.StartBattle();
    }

    public void SendContinueRound()
    {
        NetworkManager.Instance.Send(NetworkCommand.ContinueRound);
        _battleResultPanel.Continue();
    }

    public void SendRematch()
    {
        NetworkManager.Instance.Send(NetworkCommand.Rematch);
        _gameResultPanel.Rematch();
    }

    public void SendExit()
    {
        NetworkManager.Instance.Send(NetworkCommand.Exit);
        ExitGame();
    }

    public void SendDisconnect()
    {
        NetworkManager.Instance.Send(NetworkCommand.Disconnect);
        CleanupNetwork();
        SceneManager.LoadScene("BattleMenu");
    }

    private void ExitGame()
    {
        CleanupNetwork();
        _gameResultPanel.Back();
    }

    private void CleanupNetwork()
    {
        LanDiscovery.Instance.StopListening();
        LanDiscovery.Instance.StopDiscovery();
        NetworkManager.Instance.Disconnect();
        NetworkSession.Role = PlayerRole.None;
    }

    private void PlayOpponentReplaceAnimation(int slotIndex, CardData card)
    {
        RectTransform oldFrom = _opponentSelectedCard.GetSlotTransform();
        RectTransform oldTo = _opponentHandDisplay.GetSlotTransform(
            _opponentPlayer.HandManager.SelectedCard.OriginalSlotIndex);
        RectTransform newFrom = _opponentHandDisplay.GetSlotTransform(slotIndex);
        RectTransform newTo = _opponentSelectedCard.GetSlotTransform();

        _opponentSelectedCard.Clear();
        _opponentHandDisplay.HideSlot(slotIndex);

        _transitionManager.PlayReplace(
            _closedCardSprite,
            oldFrom,
            oldTo,

            _closedCardSprite,
            newFrom,
            newTo,

            () =>
            {
                _opponentPlayer.HandManager.SelectCard(card, slotIndex);
                _opponentHandDisplay.RefreshHand();
                _opponentSelectedCard.Refresh();
            });
    }

    private void PlayOpponentReturnAnimation()
    {
        if (!_opponentPlayer.HandManager.HasSelectedCard())
        {
            return;
        }

        CardData selectedCard = _opponentPlayer.HandManager.SelectedCard.Card;
        RectTransform from = _opponentSelectedCard.GetSlotTransform();
        RectTransform to = _opponentHandDisplay.GetSlotTransform(
            _opponentPlayer.HandManager.SelectedCard.OriginalSlotIndex);
        _opponentSelectedCard.Clear();

        _transitionManager.PlayReturn(
            _closedCardSprite,
            from,
            to,
            () =>
            {
                _opponentPlayer.HandManager.RestoreSelectedCard();
                _opponentHandDisplay.RefreshHand();
                _opponentSelectedCard.Refresh();
            });
    }

    private void PlayOpponentSelectAnimation(int slotIndex, CardData card)
    {
        RectTransform from = _opponentHandDisplay.GetSlotTransform(slotIndex);
        RectTransform to = _opponentSelectedCard.GetSlotTransform();
        _opponentHandDisplay.HideSlot(slotIndex);

        _transitionManager.PlaySingle(
            _closedCardSprite,
            from,
            to,
            () =>
            {
                _opponentPlayer.HandManager.SelectCard(card, slotIndex);
                _opponentHandDisplay.RefreshHand();
                _opponentSelectedCard.Refresh();
            });
    }

    private void PlayOpponentDraw(NetworkPacket packet)
    {
        string[] split = packet.Data.Split('|');
        int slotIndex = int.Parse(split[0]);
        int cardId = int.Parse(split[1]);
        CardData card = GetCardById(cardId);
        int newSlot = _opponentPlayer.HandManager.AddCard(card);

        if (newSlot < 0)
        {
            return;
        }

        AudioManager.Instance.PlaySfx(GameSfx.Message);
        _opponentMessageDisplay.Show(GameplayMessage.OpponentDraw);
        StartCoroutine(
            _drawAnimationService.PlayDraw(
                _opponentPlayer,
                _transitionManager,
                newSlot,
                () =>
                {

                }));
    }

    private void PlayOpponentReplace(NetworkPacket packet)
    {
        string[] split = packet.Data.Split('|');
        int slotIndex = int.Parse(split[0]);
        int cardId = int.Parse(split[1]);
        CardData newCard = GetCardById(cardId);

        if (newCard == null)
        {
            return;
        }

        CardData oldCard = _opponentPlayer.HandManager.Hand[slotIndex];
        AudioManager.Instance.PlaySfx(GameSfx.Message);
        _opponentMessageDisplay.Show(GameplayMessage.OpponentDraw);
        PlayOpponentReplaceAnimation(
            slotIndex,
            oldCard,
            newCard);
    }

    private void PlayOpponentReplaceAnimation(int slotIndex, CardData oldCard, CardData newCard)
    {
        RectTransform from = _opponentSelectedCard.GetSlotTransform();
        RectTransform to = _opponentHandDisplay.GetSlotTransform(slotIndex);

        _opponentSelectedCard.HideImage();
        _opponentHandDisplay.HideSlot(slotIndex);

        int completed = 0;

        void FinishOne()
        {
            completed++;

            if (completed < 2)
            {
                return;
            }

            FinishOpponentReplace(slotIndex, newCard);
        }

        _transitionManager.PlaySingle(
            _closedCardSprite,
            from,
            to,
            FinishOne);

        _destroyManager.Play(
            _closedCardSprite,
            to,
            FinishOne);
    }

    private void FinishOpponentReplace(int slotIndex, CardData newCard)
    {
        _opponentPlayer.HandManager.ReplaceCard(slotIndex, newCard);
        _opponentHandDisplay.RefreshHand();
        _opponentSelectedCard.Refresh();
        _opponentHandDisplay.ShowSlot(slotIndex);
    }

    private void PlayOpponentHeal(NetworkPacket packet)
    {
        AudioManager.Instance.PlaySfx(GameSfx.Message);
        _opponentMessageDisplay.Show(GameplayMessage.OpponentHeal);
    }

    private void PlayOpponentStatus(NetworkPacket packet)
    {
        string[] split = packet.Data.Split('|');
        int hp = int.Parse(split[0]);
        int energy = int.Parse(split[1]);
        int oldHp = _opponentPlayer.Status.HP;
        int oldEnergy = _opponentPlayer.Status.Energy;

        _opponentPlayer.Status.HP = hp;
        _opponentPlayer.Status.Energy = energy;
        _opponentPlayer.StatusDisplay.AnimateRefresh(oldHp, oldEnergy);
    }
}