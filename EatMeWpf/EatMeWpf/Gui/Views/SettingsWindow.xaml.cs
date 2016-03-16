using System.Collections.Generic;
using EatMeWpf.Gui.ViewModels;
using ff14bot.Managers;

namespace EatMeWpf.Gui.Views
{
    /// <summary>
    ///     Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        public SettingsWindow(IList<BagSlot> inventory)
        {
            InitializeComponent();
            var dataContext = new SettingsViewModel(inventory);
            DataContext = dataContext;
        }
    }
}