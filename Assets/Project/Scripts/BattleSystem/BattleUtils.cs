using TimelineHero.Character;

namespace TimelineHero.Battle
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
