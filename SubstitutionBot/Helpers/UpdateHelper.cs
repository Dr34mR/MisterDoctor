using System.Net;

namespace SubstitutionBot.Helpers
{
    internal static class UpdateHelper
    {
        internal static bool UpdateAvailable()
        {
            const string url = "https://raw.githubusercontent.com/Dr34mR/TwitchSubstitutionBot/master/SubstitutionBot/Version.txt";

            try
            {
                var webClient = new WebClient();
                var textContents = webClient.DownloadString(url);
                webClient.Dispose();

                var split = textContents.Split('.');
                if (split.Length != 4) return false;

                if (!int.TryParse(split[0], out var major)) return false;
                if (!int.TryParse(split[1], out var minor)) return false;
                if (!int.TryParse(split[2], out var build)) return false;
                if (!int.TryParse(split[3], out var revision)) return false;

                if (major > typeof(Program).Assembly.GetName().Version.Major) return true;
                if (minor > typeof(Program).Assembly.GetName().Version.Minor) return true;
                if (build > typeof(Program).Assembly.GetName().Version.Build) return true;
                if (revision > typeof(Program).Assembly.GetName().Version.Revision) return true;
            }
            catch { /**/ }
            return false;
        }
    }
}
