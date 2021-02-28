using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/Character")]
    public class CharacterAsset : ScriptableObject
    {
        public int Health;
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
                newSkillList.Add(new Skill(newActionList, skillAsset.Length, character));
            }

            character.Skills = newSkillList;
            character.Health = Health;
            character.MaxHealth = Health;
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
        public int Length;
        public ActionsList Actions;
    }

    [System.Serializable]
    public class ActionsList
    {
        public List<Action> Actions;
    }
}