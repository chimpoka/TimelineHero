using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/Character")]
    public class CharacterAsset : ScriptableObject
    {
        public int Health;
        public int Adrenaline;
        public SkillsList Skills = new SkillsList();

        public CharacterBase ToCharacter()
        {
            CharacterBase character = new CharacterBase();

            List<Skill> newSkillList = new List<Skill>();
            foreach (SkillAsset skillAsset in Skills.Skills)
            {
                List<Action> newActionList = new List<Action>();
                foreach (Action action in skillAsset.Actions.Actions)
                {
                    Action newAction = new Action(action);
                    newAction.Owner = character;
                    newAction.Position -= 1;
                    newActionList.Add(newAction);
                }
                Skill newSkill = new Skill(newActionList, skillAsset.Length, character);
                newSkill.Name = skillAsset.Name;
                newSkill.Initialize();
                newSkillList.Add(newSkill);
            }

            character.Skills = newSkillList;
            character.SkillsDict = newSkillList.ToDictionary(skill => skill.Name ?? skill.ToString(), skill => skill);
            character.Health = Health;
            character.MaxHealth = Health;
            character.Adrenaline = Adrenaline;
            character.Name = name;

            return character;
        }
    }

    [System.Serializable]
    public class SkillsList
    {
        public List<SkillAsset> Skills;
    }

    [System.Serializable]
    public class SkillAsset
    {
        public string Name;
        public int Length;
        public ActionsList Actions;
    }

    [System.Serializable]
    public class ActionsList
    {
        public List<Action> Actions;
    }
}