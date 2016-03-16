using System.Configuration;
using ff14bot.Helpers;
using Newtonsoft.Json;

namespace EatMeWpf.Gui.Models
{
    public class SettingsModel : JsonSettings
    {
        [JsonIgnore] private static SettingsModel _instance;

        public SettingsModel() : base(CharacterSettingsDirectory + "/EatMeWpf/Settings.json")
        {
        }

        public static SettingsModel Instance => _instance ?? (_instance = new SettingsModel());

        [Setting]
        public int TrueItemId { get; set; }
    }
}