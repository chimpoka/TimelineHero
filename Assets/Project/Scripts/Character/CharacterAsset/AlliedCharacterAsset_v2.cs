using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/AlliedCharacter_v2")]
    public class AlliedCharacterAsset_v2 : CharacterAssetBase
    {
        [HideIf("@this.RightHandEquipment?.Type == EquipmentType.TwoHands")]
        public EquipmentAsset LeftHandEquipment;
        [HideIf("@this.LeftHandEquipment?.Type == EquipmentType.TwoHands")]
        public EquipmentAsset RightHandEquipment;
        public EquipmentAsset BodyEquipment;
        public EquipmentAsset BootsEquipnemt;
        public EquipmentAsset ConsumableEquipment;

        public override CharacterBase ToCharacter()
        {
            CharacterBase character = new CharacterBase();

            character.CurrentEquipment = GetEquipmentSet(character);
            //character.SkillSets = SkillSets.Select(skillSetAsset => ConvertSkillSetAsset(skillSetAsset, character)).ToList();
            //character.SkillsDict = character.SkillSets[0].Skills.ToDictionary(skill => skill.Name ?? skill.ToString(), skill => skill);
            character.Health = Health;
            character.MaxHealth = Health;
            character.Adrenaline = Adrenaline;
            character.Name = name;

            return character;
        }

        protected EquipmentSet GetEquipmentSet(CharacterBase Owner)
        {
            return new EquipmentSet
            {
                LeftHandEquipment = ConvertEquipment(LeftHandEquipment, Owner),
                RightHandEquipment = ConvertEquipment(RightHandEquipment, Owner),
                BodyEquipment = ConvertEquipment(BodyEquipment, Owner),
                BootsEquipnemt = ConvertEquipment(BootsEquipnemt, Owner),
                ConsumableEquipment = ConvertEquipment(ConsumableEquipment, Owner)
            };
        }

        public static Equipment ConvertEquipment(EquipmentAsset InEquipmentAsset, CharacterBase Owner)
        {
            Equipment newEquipment = new Equipment();
            newEquipment.Name = InEquipmentAsset.name;
            newEquipment.EquipmentIcon = InEquipmentAsset.EquipmentIcon;
            newEquipment.Type = InEquipmentAsset.Type;
            newEquipment.EquipmentDecks = InEquipmentAsset.SkillSets.Select
                (skillSetAsset => new Battle_v2.EquipmentDeck(skillSetAsset.name, ConvertSkillSetAsset(skillSetAsset, Owner))).ToList();

            return newEquipment;
        }


    }
}