using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/AlliedCharacter")]
    public class AlliedCharacterAsset : CharacterAssetBase
    {
        [HideIf("@this.TwoHandsEquipment")]
        [ValidateInput("@this.LeftHandEquipment == null || this.LeftHandEquipment?.Type == EquipmentType.OneHand", "Only for <OneHand>")]
        public EquipmentAsset LeftHandEquipment;
        [HideIf("@this.TwoHandsEquipment")]
        [ValidateInput("@this.RightHandEquipment == null || this.RightHandEquipment?.Type == EquipmentType.OneHand", "Only for <OneHand>")]
        public EquipmentAsset RightHandEquipment;
        [HideIf("@this.LeftHandEquipment || this.RightHandEquipment")]
        [ValidateInput("@this.TwoHandsEquipment == null || this.TwoHandsEquipment?.Type == EquipmentType.TwoHands", "Only for <TwoHands>")]
        public EquipmentAsset TwoHandsEquipment;
        [ValidateInput("@this.BodyEquipment == null || this.BodyEquipment?.Type == EquipmentType.Body", "Only for <Body>")]
        public EquipmentAsset BodyEquipment;
        [ValidateInput("@this.BootsEquipnemt == null || this.BootsEquipnemt?.Type == EquipmentType.Boots", "Only for <Boots>")]
        public EquipmentAsset BootsEquipnemt;
        [ValidateInput("@this.ConsumableEquipment == null || this.ConsumableEquipment?.Type == EquipmentType.Consumable", "Only for <Consumable>")]
        public EquipmentAsset ConsumableEquipment;

        public override CharacterBase ToCharacter()
        {
            CharacterBase character = new CharacterBase();

            character.CurrentEquipment = GetEquipmentSet(character);
            character.Health = Health;
            character.MaxHealth = Health;
            character.Adrenaline = Adrenaline;
            character.Name = name;

            return character;
        }

        protected EquipmentSet GetEquipmentSet(CharacterBase Owner)
        {
            var newEquipmentSet = new EquipmentSet()
            { 
                ConvertEquipment(LeftHandEquipment, Owner),
                ConvertEquipment(RightHandEquipment, Owner),
                ConvertEquipment(TwoHandsEquipment, Owner),
                ConvertEquipment(BodyEquipment, Owner),
                ConvertEquipment(BootsEquipnemt, Owner),
                ConvertEquipment(ConsumableEquipment, Owner)
            };

            return newEquipmentSet;
        }

        public static Equipment ConvertEquipment(EquipmentAsset InEquipmentAsset, CharacterBase Owner)
        {
            if (!InEquipmentAsset)
                return null;

            Equipment newEquipment = new Equipment();
            newEquipment.Name = InEquipmentAsset.name;
            newEquipment.EquipmentIcon = InEquipmentAsset.EquipmentIcon;
            newEquipment.Type = InEquipmentAsset.Type;
            newEquipment.EquipmentDecks = InEquipmentAsset.SkillSets.Select
                (skillSetAsset => new Battle.EquipmentDeck(ConvertSkillSetAsset(skillSetAsset, Owner))).ToList();

            return newEquipment;
        }


    }
}