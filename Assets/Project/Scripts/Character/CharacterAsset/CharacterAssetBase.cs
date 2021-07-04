using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TimelineHero.Character
{
    public class CharacterAssetBase : ScriptableObject
    {
        public int Health;
        public int Adrenaline;

        protected void SetBaseData(CharacterBase CharacterRef)
        {
            CharacterRef.Health = Health;
            CharacterRef.MaxHealth = Health;
            CharacterRef.Adrenaline = Adrenaline;
            CharacterRef.Name = name;
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