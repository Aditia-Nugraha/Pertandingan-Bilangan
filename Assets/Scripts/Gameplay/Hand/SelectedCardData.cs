using UnityEngine;

[System.Serializable]
public class SelectedCardData
{
    public CardData Card;
    public int OriginalSlotIndex = -1;
    public bool HasCard => Card != null;

    public void Clear()
    {
        Card = null;
        OriginalSlotIndex = -1;
    }
}