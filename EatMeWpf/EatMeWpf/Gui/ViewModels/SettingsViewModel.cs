using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EatMeWpf.Gui.Commands;
using EatMeWpf.Gui.Models;
using EatMeWpf.Utilities;
using ff14bot.Managers;

namespace EatMeWpf.Gui.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel(IList<BagSlot> inventory)
        {
            FoodItems = inventory.Any()
                ? inventory.FoodItems().GroupBy(x => x.TrueItemId).Select(y => y.FirstOrDefault())
                : new List<BagSlot>();

            var item = FoodItems.FirstOrDefault(x => x.TrueItemId == SettingsModel.Instance.TrueItemId);
            if (item != null)
            {
                FoodItem = item;
            }
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new DelegateCommand(Close));

        public IEnumerable<BagSlot> FoodItems { get; }

        private BagSlot _foodItem;
        public BagSlot FoodItem
        {
            get { return _foodItem; }
            set
            {
                _foodItem = value;
                if (SettingsModel.Instance.TrueItemId != (int) value.TrueItemId)
                {
                    SettingsModel.Instance.TrueItemId = (int) value.TrueItemId;
                    SettingsModel.Instance.Save();
                }
                OnPropertyChanged();
            }
        }

        private void Close()
        {
            EatMe.SettingsWindow.Close();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}