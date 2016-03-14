using System;
using System.Linq;
using System.Windows.Forms;
using EatMe.Settings;
using EatMe.Utilities;
using ff14bot.Managers;

namespace EatMe.Gui
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            comboBox1.DataSource = InventoryManager.FilledSlots.FoodItems().Select(x => x.Item.EnglishName + "(HQ: " + x.Item.IsHighQuality.ToString() + ")").ToList(); 
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            EatFoodSettings.Instance.FoodName = comboBox1.SelectedValue.ToString();
            EatFoodSettings.Instance.Save();
        }
    }
}