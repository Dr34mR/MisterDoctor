using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using MisterDoctor.Classes;
using MisterDoctor.Helpers;
using MisterDoctor.Plugins;
using MisterDoctor.Plugins.Classes;

namespace MisterDoctor.Managers
{
    internal class PluginManager
    {
        [ImportMany(typeof(Plugin))]
        internal List<Plugin> Plugins { get; private set; } = new();

        internal static List<Plugin> LoadedPlugins
        {
            get
            {
                var returnList = new List<Plugin>();

                foreach(var state in Instance._pluginStates.OrderBy(i => i.Sequence))
                {
                    var matchingPlugin = Instance.Plugins.FirstOrDefault(i => i.UniqueId == state.Id);
                    if (matchingPlugin == null) continue;
                    returnList.Add(matchingPlugin);
                }

                return returnList;
            }
        }

        // -------------- //

        private const string PluginDirectory = "Plugins";

        private readonly object _threadLock = new();

        private static readonly PluginManager Instance = new();

        private PluginStates _pluginStates;

        // -------------- //

        public bool Initialized { get; private set; }

        public static void Initialize()
        {
            if (Instance.Initialized) return;
            Instance._load();
        }

        private void _load()
        {
            // Load Stuff Via MEF

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var pluginFolder = Path.Combine(baseDirectory, PluginDirectory);

            if (!Directory.Exists(pluginFolder))
            {
                Directory.CreateDirectory(pluginFolder);
            }

            var loadable = new DirectoryCatalog(pluginFolder);
            var catalog = new AggregateCatalog(loadable);

            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);

            // Now call initialize on those needed

            var needsLoading = Plugins.Where(i => !i.IsInitialized).ToList();

            foreach (var loadPlugin in needsLoading)
            {
                loadPlugin.Initialize();
            }

            foreach (var plugin in Plugins)
            {
                plugin.SendMessageHandler += Plugin_SendMessageHandler;
                plugin.TimeoutHandler += Plugin_TimeoutHandler;

                var existingSettings = DbHelper.ReadPluginSettings(plugin);
                plugin.LoadSettings(existingSettings);
            }

            Initialized = true;

            RefreshStates();
        }

        // -------------- //

        public static bool IsPluginEnabled(Plugin plugin)
        {
            if (plugin == null) return false;
            if (Instance._pluginStates == null) return false;

            var enabled = Instance._pluginStates.Where(i => i.Enabled);
            return enabled.Any(i => i.Id == plugin.UniqueId);
        }

        public static int PluginSequence(Plugin plugin)
        {
            if (plugin == null) return 99;
            if (Instance._pluginStates == null) return 99;

            return Instance._pluginStates.FirstOrDefault(i => i.Id == plugin.UniqueId)?.Sequence ?? 99;
        }

        public static void MovePluginUp(Plugin plugin)
        {
            if (plugin == null) return;

            var currentState = Instance._pluginStates.FirstOrDefault(i => i.Id == plugin.UniqueId);
            if (currentState == null) return;

            var swapWith = Instance._pluginStates.FirstOrDefault(i => i.Sequence == currentState.Sequence - 1);
            if (swapWith == null) return;

            var temp = currentState.Sequence;
            currentState.Sequence = swapWith.Sequence;
            swapWith.Sequence = temp;

            DbHelper.SavePluginState(Instance._pluginStates);
        }

        public static void MovePluginDown(Plugin plugin)
        {
            if (plugin == null) return;

            var currentState = Instance._pluginStates.FirstOrDefault(i => i.Id == plugin.UniqueId);
            if (currentState == null) return;

            var swapWith = Instance._pluginStates.FirstOrDefault(i => i.Sequence == currentState.Sequence + 1);
            if (swapWith == null) return;

            var temp = currentState.Sequence;
            currentState.Sequence = swapWith.Sequence;
            swapWith.Sequence = temp;

            DbHelper.SavePluginState(Instance._pluginStates);
        }

        public static void SetPluginState(Plugin plugin, bool enable)
        {
            if (!Instance.Initialized) return;

            lock (Instance._threadLock)
            {
                var state = Instance._pluginStates.FirstOrDefault(i => i.Id == plugin.UniqueId);

                if (state == null)
                {
                    state = new PluginState
                    {
                        Id = plugin.UniqueId
                    };
                    Instance._pluginStates.Add(state);
                }

                state.Enabled = enable;

                DbHelper.SavePluginState(Instance._pluginStates);
            }
        }

        public void RefreshStates()
        {
            if (!Instance.Initialized) return;

            lock (Instance._threadLock)
            {
                _pluginStates = DbHelper.ReadPluginStates() ?? new PluginStates();

                var resave = false;

                foreach (var plugin in Instance.Plugins.Where(plugin => _pluginStates.All(i => i.Id != plugin.UniqueId)))
                {
                    _pluginStates.Add(new PluginState
                    {
                        Enabled = false,
                        Id = plugin.UniqueId,
                        Sequence = int.MaxValue
                    });
                    resave = true;
                }

                // Double check plugin sequences

                var seqNumber = -1;
                foreach (var plugin in Instance._pluginStates)
                {
                    seqNumber += 1;
                    if (plugin.Sequence == seqNumber) continue;
                    plugin.Sequence = seqNumber;
                    resave = true;
                }

                if (!resave) return;
                DbHelper.SavePluginState(_pluginStates);
            }
        }

        // -------------- //

        public static event EventHandler<SendMessageArgs> SendMessageHandler;

        public static event EventHandler<TimeoutArgs> TimeoutHandler;

        public static void MessageToPlugins(DigestMessage message)
        {
            if (!Instance.Initialized) return;

            lock (Instance._threadLock)
            {
                var enabledPlugins = Instance._pluginStates
                    .Where(i => i.Enabled)
                    .OrderBy(i => i.Sequence);

                foreach (var enabledPlugin in enabledPlugins)
                {
                    var plugin = Instance.Plugins.FirstOrDefault(i => i.UniqueId == enabledPlugin.Id);
                    if (plugin == null) continue;
                    var wasProcessed = plugin.ReceiveMessage(message);
                    if (wasProcessed) break;
                }
            }
        }

        private static void Plugin_TimeoutHandler(object sender, TimeoutArgs e)
        {
            if (e == null) return;
            if (string.IsNullOrEmpty(e.Username)) return;

            TimeoutHandler?.Invoke(sender, e);
        }

        private static void Plugin_SendMessageHandler(object sender, SendMessageArgs e)
        {
            if (e == null) return;
            SendMessageHandler?.Invoke(sender, e);
        }
    }
}