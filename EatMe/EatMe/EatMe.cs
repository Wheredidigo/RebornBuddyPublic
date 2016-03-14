using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using EatMe.Gui;
using EatMe.Logic;
using EatMe.Utilities;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Managers;

namespace EatMe
{
    public class EatMe : BotPlugin
    {
        #region Overrides
        /// <summary>
        ///     Take credit for what you write!!!
        /// </summary>
        public override string Author
        {
            get { return "Wheredidigo"; }
        }
        /// <summary>
        ///     Version Number for the Plugin
        /// </summary>
        public override Version Version
        {
            get { return new Version(1, 0); }
        }
        /// <summary>
        ///     Name of the Plugin
        /// </summary>
        public override string Name
        {
            get { return "Eat Me"; }
        }

        /// <summary>
        ///     Add all of the Logic that needs to be hooked to the main TreeRoot
        /// </summary>
        public override void OnInitialize()
        {
            Logic.Add(new EatFood());
        }

        /// <summary>
        ///     Add event handlers for the various events we want to use. Also add hooks if the bot is already running.
        /// </summary>
        public override void OnEnabled()
        {
            TreeRoot.OnStart += OnBotStart;
            TreeRoot.OnStop += OnBotStop;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            if (TreeRoot.IsRunning)
            {
                AddHooks();
            }
        }

        /// <summary>
        ///     Remove event handlers for the various events we used. Also remove hooks from the main TreeRoot
        /// </summary>
        public override void OnDisabled()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            TreeHooks.Instance.OnHooksCleared -= OnHooksCleared;
            RemoveHooks();
        }

        /// <summary>
        /// Tell RB that we want to have a Settings Button for this Plugin
        /// </summary>
        public override bool WantButton
        {
            get { return true; }
        }

        /// <summary>
        /// Method that gets called when the Settings Button for this Plugin gets clicked
        /// </summary>
        public override void OnButtonPress()
        {
            SettingsForm.ShowDialog();
        }

        /// <summary>
        /// Text to diplay on the Settings Button in the RB Window
        /// </summary>
        public override string ButtonText
        {
            get { return "Eat Me"; }
        }

        #endregion

        #region Properties

        private SettingsForm _settingsForm;

        private SettingsForm SettingsForm
        {
            get
            {
                if (_settingsForm == null || _settingsForm.IsDisposed || _settingsForm.Disposing)
                {
                    _settingsForm = new SettingsForm();
                    _settingsForm.Closed += (sender, args) => _settingsForm = null;
                }
                return _settingsForm;
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
            Logger.Log(string.Format("{0} was called. Adding Hooks now!", methodName));
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
            Logger.Log(string.Format("{0} was called. Adding Hooks now!", methodName));
            foreach (var logic in Logic)
            {
                TreeHooks.Instance.RemoveHook("TreeStart", logic.Execute);
            }
        }

        #endregion

        #region Logic

        private IList<ILogic> _logic;

        private IList<ILogic> Logic
        {
            get { return _logic ?? (_logic = new List<ILogic>()); }
        }

        #endregion
    }
}