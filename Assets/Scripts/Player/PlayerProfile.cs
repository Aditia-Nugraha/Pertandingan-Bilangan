public static class PlayerProfile
{
    //GameMode
    public static GameMode CurrentGameMode = GameMode.PlayerVsComputer;
    public static PlayerSide CurrentViewingSide = PlayerSide.Player1;

    // Constants
    public const int MaxHP = 1000;
    public const int MaxEnergy = 30;

    public const int StartHP = MaxHP;
    public const int StartEnergy = 10;

    // Player Identity
    public static string Player1Name = "Player 1";
    public static string Player2Name = "Computer";
}