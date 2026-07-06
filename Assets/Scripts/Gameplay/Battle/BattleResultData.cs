using System;

[Serializable]
public class BattleResultData
{
    public int Round;

    public CardData Player1Card;
    public CardData Player2Card;

    public BattleOutcome Outcome;

    public PlayerBattleResult Player1 = new();
    public PlayerBattleResult Player2 = new();
}