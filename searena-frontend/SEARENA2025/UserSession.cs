namespace SEARENA2025
{
    public static class UserSession
    {
        public static int UserId { get; private set; }
        public static string Username { get; private set; }

        public static bool IsLoggedIn => UserId > 0 && !string.IsNullOrWhiteSpace(Username);

        public static void SetUser(int userId, string username)
        {
            UserId = userId;
            Username = username ?? "";
        }

        public static void Clear()
        {
            UserId = 0;
            Username = "";
        }
    }
}
