using UnityEngine;

[CreateAssetMenu(
    fileName = "CardData",
    menuName = "Pertandingan Bilangan/Card Data")]
    
public class CardData : ScriptableObject
{
    [Header("Identity")]
    public int CardId;

    [Header("Representation")]
    public CardRepresentation Representation;

    [Header("Gameplay")]
    public float Value;
    public int Attack;

    [Header("Visual")]
    public Sprite CardSprite;
}