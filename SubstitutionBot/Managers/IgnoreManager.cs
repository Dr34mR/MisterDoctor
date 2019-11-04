using System.Collections.Generic;
using SubstitutionBot.Classes;
using SubstitutionBot.Helpers;

namespace SubstitutionBot.Managers
{
    internal class IgnoreManager
    {
        private bool _initialized;
        private readonly HashSet<string> _ignoreList = new HashSet<string>();

        private static IgnoreManager Manager { get; } = new IgnoreManager();

        private IgnoreManager()
        {
            
        }

        internal static void Initialize()
        {
            if (Manager._initialized) return;
            
            var ignoreList = DbHelper.IgnoreGet();

            foreach (var user in ignoreList)
            {
                Manager._ignoreList.Add(user.Username);
            }

            Manager._ignoreList.TrimExcess();

            Manager._initialized = true;
        }

        internal static bool IgnoreUser(string username)
        {
            var lowerUser = username.Trim().ToLower();
            return Manager._ignoreList.Contains(lowerUser);
        }

        internal static void AddIgnore(string username)
        {
            var lowerUser = username.Trim().ToLower();
            if (Manager._ignoreList.Contains(lowerUser)) return;

            Manager._ignoreList.Add(lowerUser);
            DbHelper.IgnoreAdd(lowerUser);
        }

        internal static void RemoveIgnore(string username)
        {
            var lowerUser = username.Trim().ToLower();
            if (!Manager._ignoreList.Contains(lowerUser)) return;
            
            Manager._ignoreList.Remove(lowerUser);
            DbHelper.IgnoreDelete(lowerUser);
        }

        internal static IEnumerable<User> GetUsers()
        {
            return DbHelper.IgnoreGet();
        }
    }
}
