using System.Reflection;

namespace jwtApi.Presentation.Config
{
    public static class AppConfig
    {
        public static string Name => "JwtApi";

        public static string GetLongVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetShortVersion()
        {
            return $"v{Assembly.GetExecutingAssembly().GetName().Version.Major}";
        }

        public static int GetMajorVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.Major;
        }

        public static int GetMinorVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.Minor;
        }
    }
}
