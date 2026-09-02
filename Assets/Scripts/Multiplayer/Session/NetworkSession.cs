public static class NetworkSession
{
    public static bool Connected;
    public static PlayerRole Role;
    public static PlayerSide LocalPlayer;
    public static string LocalPlayerName;
    public static string RemotePlayerName;
    public static int MatchSeed;

    public static void Reset()
    {
        Connected = false;
        Role = PlayerRole.Host;
        LocalPlayer = PlayerSide.Player1;
        LocalPlayerName = string.Empty;
        RemotePlayerName = string.Empty;
        MatchSeed = 0;
    }
}