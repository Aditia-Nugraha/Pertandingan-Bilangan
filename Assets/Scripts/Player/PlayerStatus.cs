using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public int HP;
    public int Energy;

    public void Reset()
    {
        HP = PlayerProfile.StartHP;
        Energy = PlayerProfile.StartEnergy;
    }
}