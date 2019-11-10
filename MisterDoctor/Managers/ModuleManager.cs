using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Component;

namespace MisterDoctor.Managers
{
    public class ModuleManager
    {
        [ImportMany(typeof(ITwitchModule))]
        private IEnumerable<ITwitchModule> _modules;

        private static readonly ModuleManager Instance = new ModuleManager();

        private ModuleManager()
        {

        }

        private void MefLoad()
        {
            // Figure out the current directory

            var cdInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var loadable = new DirectoryCatalog(cdInfo.FullName);
            var catalog = new AggregateCatalog(loadable);

            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }

        public static void LoadConverters()
        {
            Instance.MefLoad();
        }

        public static List<string> SendMessages(string message)
        {
            var returnMessages = new List<string>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var module in Instance._modules)
            {
                var returnMessage = module.DigestMessage(message);
                if (string.IsNullOrEmpty(returnMessage)) continue;
                returnMessages.Add(returnMessage);
            }

            return returnMessages;
        }
    }
}
