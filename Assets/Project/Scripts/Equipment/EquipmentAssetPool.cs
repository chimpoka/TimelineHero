using System.Collections.Generic;
using System.Linq;
using TimelineHero.Core;
using TimelineHero.Core.Utils;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/EquipmentAssetPool")]
    public class EquipmentAssetPool : SingletonScriptableObject<EquipmentAssetPool>
    {
        [SerializeField] private List<EquipmentAsset> OneHandEquipment;
        [SerializeField] private List<EquipmentAsset> TwoHandsEquipment;
        [SerializeField] private List<EquipmentAsset> BodyEquipment;
        [SerializeField] private List<EquipmentAsset> BootsEquipment;
        [SerializeField] private List<EquipmentAsset> ConsumableEquipment;

        public List<EquipmentAsset> GetAllEquipmentAssets()
        { 
            return CoreUtils.Combine(OneHandEquipment, TwoHandsEquipment, BodyEquipment, BootsEquipment, ConsumableEquipment).ToList();
        }
    }

    public static class EquipmentPool
    {
        private static List<Equipment> _EquipmentPool;

        public static List<Equipment> GetAllEquipment()
        {
            if (_EquipmentPool == null)
            {
                CharacterBase character = CharacterPool.GetCurrentAlliedCharacter();
                List<EquipmentAsset> assets = EquipmentAssetPool.Get().GetAllEquipmentAssets();
                _EquipmentPool = assets.Select(asset => AlliedCharacterAsset.ConvertEquipment(asset, character)).ToList();
            }

            return _EquipmentPool;
        }

        public static List<Equipment> GetAllEquipmentOfType(EquipmentType Type)
        {
            return GetAllEquipment().FindAll(x => x.Type == Type);
        }

        public static List<Equipment> GetAllEquipmentWeapon()
        {
            return GetAllEquipment().FindAll(x => x.Type == EquipmentType.OneHand || x.Type == EquipmentType.TwoHands);
        }
    }
}