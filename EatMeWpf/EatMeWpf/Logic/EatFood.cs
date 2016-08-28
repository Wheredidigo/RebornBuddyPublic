using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using EatMeWpf.Gui.Models;
using EatMeWpf.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;

namespace EatMeWpf.Logic
{
    public class EatFood : BaseLogic
    {
        /// <summary>
        ///     Return true if we do not have the "Well Fed" buff
        /// </summary>
        protected override bool NeedToStart => !Core.Me.HasAura(48);

        /// <summary>
        ///     Logic for eating food from our Inventory
        /// </summary>
        protected override async Task<bool> Start()
        {
            //Make sure that the user has selected a Food Item from the settings.
            if (SettingsModel.Instance.TrueItemId == 0)
            {
                Logger.Log("Please select a new Food Item from the settings window.");
                return false;
            }

            var item = InventoryManager.FilledSlots.FoodItems().OrderBy(x => x.Count).FirstOrDefault(x => x.RawItemId == SettingsModel.Instance.TrueItemId);

            if (item == null)
            {
                Logger.Log($"Unable to find food (TrueItemId: {SettingsModel.Instance.TrueItemId}) in your inventory. Resetting your Food Setting to 0.");
                SettingsModel.Instance.TrueItemId = 0;
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

            Logger.Log("Now eating " + item.Item.CurrentLocaleName);
            item.UseItem();
            await Coroutine.Wait(5000, () => Core.Me.HasAura(48));

            return true;
        }
    }
}