using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using EatMe.Settings;
using EatMe.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;

namespace EatMe.Logic
{
    public class EatFood : BaseLogic
    {
        /// <summary>
        ///     Return true if we do not have the "Well Fed" buff
        /// </summary>
        protected override bool NeedToStart
        {
            get { return Core.Me.HasAura(48); }
        }
        /// <summary>
        ///     Logic for eating food from our Inventory
        /// </summary>
        protected override async Task<bool> Start()
        {
            //Make sure that the user has selected a Food Item from the settings.
            if (string.IsNullOrEmpty(EatFoodSettings.Instance.FoodName))
            {
                Logger.Log("Please select a new Food Item from the settings window.");
                return false;
            }

            var foodSetting = EatFoodSettings.Instance.FoodName;
            var index = foodSetting.IndexOf('(');
            var food = index > 0 ? foodSetting.Substring(0, index) : foodSetting;

            var item = InventoryManager.FilledSlots.FoodItems().FirstOrDefault(x => x.Item.EnglishName == food);

            if (item == null)
            {
                Logger.Log(string.Format("Unable to find {0} item in your inventory. Resetting your FoodName Setting to null.", food));
                EatFoodSettings.Instance.FoodName = string.Empty;
                return false;
            }

            //Handle Fishing
            if (FishingManager.State != FishingState.None)
            {
                Logger.Log("We are currently fishing. Stopping fishing so that we can eat food now.");
                Actionmanager.DoAction(299, Core.Me);
                await Coroutine.Wait(5000, () => FishingManager.State == FishingState.None);
            }

            //Handle Gathering
            if (GatheringManager.WindowOpen)
            {
                Logger.Log("Currently Gathering Items, waiting until we are done.");
                return false;
            }

            //Handle Mounted
            if (Core.Me.IsMounted)
            {
                if (MovementManager.IsFlying)
                {
                    Logger.Log("Currently Flying, waiting until we have landed.");
                    return false;
                }
                Logger.Log("Dismounting so we can eat food.");
                await CommonTasks.StopAndDismount();
            }

            Logger.Log("Now eating " + item.Item.EnglishName);
            item.UseItem();
            await Coroutine.Wait(5000, () => Core.Me.HasAura(48));

            return true;
        }
    }
}