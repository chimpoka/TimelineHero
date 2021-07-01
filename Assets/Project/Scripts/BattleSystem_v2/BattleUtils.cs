using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineHero.Character;

namespace TimelineHero.Battle_v2
{
    static class BattleUtils
    {
        public static EquipmentType ConvertEquipmentSlotToType(EquipmentSlot Slot)
        {
            switch (Slot)
            {
                case EquipmentSlot.LeftHand: return EquipmentType.OneHand;
                case EquipmentSlot.RightHand: return EquipmentType.OneHand;
                case EquipmentSlot.TwoHands: return EquipmentType.TwoHands;
                case EquipmentSlot.Body: return EquipmentType.Body;
                case EquipmentSlot.Boots: return EquipmentType.Boots;
                case EquipmentSlot.Consumable: return EquipmentType.Consumable;
                default: return EquipmentType.OneHand;
            }
        }
    }
}
