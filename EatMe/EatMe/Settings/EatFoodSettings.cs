using System.ComponentModel;
using System.Configuration;
using ff14bot.Helpers;
using Newtonsoft.Json;

namespace EatMe.Settings
{
    public class EatFoodSettings : JsonSettings
    {   
        public EatFoodSettings() : base(CharacterSettingsDirectory + "/EatMe/GourmetGuy.json")
        {
        }

        [JsonIgnore]
        private static EatFoodSettings _instance;
        public static EatFoodSettings Instance
        {
            get { return _instance ?? (_instance = new EatFoodSettings()); }
        }

        [Setting]
        [DefaultValue("")]
        public string FoodName { get; set; }
    }
}