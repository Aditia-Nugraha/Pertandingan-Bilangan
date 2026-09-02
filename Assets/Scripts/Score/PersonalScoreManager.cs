using UnityEngine;

public class PersonalScoreManager : MonoBehaviour
{
    private const string WinCountKey = "WinCount";
    private const string LoseCountKey = "LoseCount";
    private const string TotalScoreKey = "TotalScore";

    public int WinCount { get; private set; }
    public int LoseCount { get; private set; }
    public int TotalScore { get; private set; }

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        WinCount = PlayerPrefs.GetInt(WinCountKey, 0);
        LoseCount = PlayerPrefs.GetInt(LoseCountKey, 0);
        TotalScore = PlayerPrefs.GetInt(TotalScoreKey, 0);
    }

    public void AddWin(int remainingHP)
    {
        WinCount++;
        TotalScore += remainingHP;
        SaveData();
    }

    public void AddLose()
    {
        LoseCount++;
        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(WinCountKey, WinCount);
        PlayerPrefs.SetInt(LoseCountKey, LoseCount);
        PlayerPrefs.SetInt(TotalScoreKey, TotalScore);
        PlayerPrefs.Save();
    }
}