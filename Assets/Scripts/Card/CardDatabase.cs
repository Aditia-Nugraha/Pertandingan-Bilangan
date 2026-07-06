using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CardDatabase",
    menuName = "Pertandingan Bilangan/Card Database")]

public class CardDatabase : ScriptableObject
{
    [SerializeField] private List<CardData> _cards = new();
    public IReadOnlyList<CardData> Cards => _cards;
    
    public CardData GetCardById(int cardId)
    {
        return _cards.Find(card => card.CardId == cardId);
    }
}