using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EatMeWpf.Gui.Views;
using EatMeWpf.Logic;
using EatMeWpf.Utilities;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Managers;

namespace EatMeWpf
{
    public class EatMe
    {
        #region Public Methods / Properties

        /// <summary>
        ///     Add all of the Logic that needs to be hooked to the main TreeRoot
        /// </summary>
        public void OnInitialize()
        {
            Logic.Add(new EatFood());
        }

        /// <summary>
        ///     Add event handlers for the various events we want to use. Also add hooks if the bot is already running.
        /// </summary>
        public void OnEnabled()
        {
            TreeRoot.OnStart += OnBotStart;
            TreeRoot.OnStop += OnBotStop;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            //You only want to add the hook immediately when the plugin is enabled if the TreeRoot is currently running.
            if (TreeRoot.IsRunning)
            {
                AddHooks();
            }
        }

        /// <summary>
        ///     Remove event handlers for the various events we used. Also remove hooks from the main TreeRoot
        /// </summary>
        public void OnDisabled()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            TreeHooks.Instance.OnHooksCleared -= OnHooksCleared;
            RemoveHooks();
        }

        /// <summary>
        ///     Method that gets called when the Settings Button for this Plugin gets clicked
        /// </summary>
        public void OnButtonPress()
        {
            SettingsWindow.ShowDialog();
        }

        /// <summary>
        ///     Take credit for the work you do!
        /// </summary>
        public string Author => "Wheredidigo";

        /// <summary>
        ///     Explicit Version of the Plugin.
        /// </summary>
        public Version Version => new Version(1,0);

        /// <summary>
        ///     Name your Plugin!
        /// </summary>
        public string Name => "Eat Me Wpf";

        /// <summary>
        ///     Tell RB that we want to have a Settings Button for this Plugin
        /// </summary>
        public bool WantButton => true;

        /// <summary>
        ///     Text to diplay on the Settings Button in the RB Window
        /// </summary>
        public string ButtonText => Name;

        #endregion

        #region Properties

        private static SettingsWindow _settingsWindow;

        internal static SettingsWindow SettingsWindow
        {
            get
            {
                if (_settingsWindow == null)
                {
                    //We need to get the list of inventory items while we are still on the main thread and then pass them into the Gui so that we avoid threading issues.
                    _settingsWindow = new SettingsWindow(InventoryManager.FilledSlots.ToList());
                    _settingsWindow.Closed += (sender, args) => _settingsWindow = null;
                }
                return _settingsWindow;
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Event Handler for when the event OnBotStart gets fired. We want to make sure that if our Plugin is still enabled
        ///     that we add the logic back to the main TreeRoot
        /// </summary>
        private void OnBotStart(BotBase bot)
        {
            if (PluginManager.GetEnabledPlugins().Contains(Name))
            {
                AddHooks();
            }
        }

        /// <summary>
        ///     Event Handler for when the event OnBotStop gets fired. We want to make sure to remove our hooks when the bot stops.
        /// </summary>
        private void OnBotStop(BotBase bot)
        {
            RemoveHooks();
        }

        /// <summary>
        ///     Event Handler for when the event OnHooksCleared gets fired. We want to make sure that if our Plugin is still
        ///     enabled that we add the logic back to the main TreeRoot
        /// </summary>
        private void OnHooksCleared(object sender, EventArgs e)
        {
            if (PluginManager.GetEnabledPlugins().Contains(Name))
            {
                AddHooks();
            }
        }

        #endregion

        #region Hooks

        /// <summary>
        ///     Add everything that is in the Logic property to the main TreeRoot
        /// </summary>
        private void AddHooks([CallerMemberName] string methodName = null)
        {
            Logger.Log($"{methodName} was called. Adding Hooks now!");
            var counter = 0;
            foreach (var logic in Logic)
            {
                TreeHooks.Instance.InsertHook("TreeStart", counter, logic.Execute);
                counter++;
            }
        }

        /// <summary>
        ///     Remove everything that is in the Logic property to the main TreeRoot
        /// </summary>
        private void RemoveHooks([CallerMemberName] string methodName = null)
        {
            Logger.Log($"{methodName} was called. Adding Hooks now!");
            foreach (var logic in Logic)
            {
                TreeHooks.Instance.RemoveHook("TreeStart", logic.Execute);
            }
        }

        #endregion

        #region Logic

        private IList<ILogic> _logic;
        private IList<ILogic> Logic => _logic ?? (_logic = new List<ILogic>());

        #endregion
    }
}