using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;


namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/CharacterV2")]
    public class CharacterAssetV2 : ScriptableObject
    {
        public int Health;
        public int Adrenaline;
        public List<SkillAssetV2> Skills = new List<SkillAssetV2>();

        public CharacterBase ToCharacter()
        {
            CharacterBase character = new CharacterBase();

            List<Skill> newSkillList = new List<Skill>();
            foreach (SkillAssetV2 skillAsset in Skills)
            {
                List<Action> newActionList = new List<Action>();
                foreach (Action action in skillAsset.Actions)
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
    public class SkillAssetV2
    {
        [GUIColor(0.0f, 1.0f, 0.0f, 1.0f)]
        public string Name;
        public int Length;
        public List<Action> Actions;
    }
}