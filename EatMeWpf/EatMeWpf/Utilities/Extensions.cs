using System.Collections.Generic;
using System.Linq;
using ff14bot.Managers;

namespace EatMeWpf.Utilities
{
    public static class Extensions
    {
        /// <summary>
        ///     Filter bagslots to only those that can be used to give the Well Fed Buff
        /// </summary>
        public static IEnumerable<BagSlot> FoodItems(this IEnumerable<BagSlot> inventory)
        {
            return InventoryManager.FilledSlots.Where(x => !x.IsCollectable && (int) x.Item.ItemRole == 5);
        }
    }
}