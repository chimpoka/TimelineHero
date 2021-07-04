using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/Character")]
    public class CharacterAssetBase : ScriptableObject
    {
        public int Health;
        public int Adrenaline;

        public List<SkillSetAsset> SkillSets = new List<SkillSetAsset>();

        public virtual CharacterBase ToCharacter()
        {
            return ConvertCharacterAsset();
        }

        protected CharacterBase ConvertCharacterAsset()
        {
            CharacterBase character = new CharacterBase();

            character.SkillSets = SkillSets.Select(skillSetAsset => ConvertSkillSetAsset(skillSetAsset, character)).ToList();
            character.SkillsDict = character.SkillSets[0].Skills.ToDictionary(skill => skill.Name ?? skill.ToString(), skill => skill);
            character.Health = Health;
            character.MaxHealth = Health;
            character.Adrenaline = Adrenaline;
            character.Name = name;

            return character;
        }

        public static SkillSet ConvertSkillSetAsset(SkillSetAsset InSkillSetAsset, CharacterBase Owner)
        {
            SkillSet newSkillSet = new SkillSet();
            newSkillSet.SkillName = InSkillSetAsset.SkillName;
            newSkillSet.Skills = InSkillSetAsset.Skills.Select(skillAsset => ConvertSkillAsset(skillAsset, Owner)).ToList();

            return newSkillSet;
        }

        public static Skill ConvertSkillAsset(SkillAsset InSkillAsset, CharacterBase Owner)
        {
            List<Action> newActionList = InSkillAsset.Actions.Select(action => ConvertActionAsset(action, Owner)).ToList();
            Skill newSkill = new Skill(newActionList, InSkillAsset.Length, Owner);
            newSkill.Name = InSkillAsset.Name;
            newSkill.Initialize();

            return newSkill;
        }

        public static Action ConvertActionAsset(Action InAction, CharacterBase Owner)
        {
            Action newAction = new Action(InAction);
            newAction.Owner = Owner;
            newAction.Position -= 1;

            return newAction;
        }
    }

    [System.Serializable]
    public class SkillAsset
    {
        [GUIColor(0.0f, 1.0f, 0.0f, 1.0f)]
        public string Name;
        public int Length;
        public List<Action> Actions;
    }
}