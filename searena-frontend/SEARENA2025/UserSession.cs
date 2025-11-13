using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SEARENA2025
{
    public static class UserSession
    {
        public static int UserId { get; private set; }
        public static string Username { get; private set; } = "";
        public static string Email { get; private set; } = "";

        public static bool IsLoggedIn => UserId > 0 && !string.IsNullOrWhiteSpace(Username);

        public static void SetUser(int userId, string username, string email)
        {
            UserId = userId;
            Username = username ?? "";
            Email = email ?? "";
        }

        public static void Clear()
        {
            UserId = 0;
            Username = "";
            Email = "";
        }
    }
}
