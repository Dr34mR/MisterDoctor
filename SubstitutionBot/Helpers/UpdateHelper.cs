using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SubstitutionBot.Helpers
{
    internal static class UpdateHelper
    {
        internal static bool UpdateAvailable()
        {
            try
            {
                var vlient = new WebClient();
                var theTextFile = vlient.DownloadString("The Direct Address");
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal static bool LaunchUrl()
        {

        }
    }
}
