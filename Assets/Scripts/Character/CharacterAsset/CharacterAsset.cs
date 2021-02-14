using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Character
{
    [CreateAssetMenu(menuName = "ScriptableObject/Character")]
    public class CharacterAsset : ScriptableObject
    {
        public SkillsList Skills = new SkillsList();

        public CharacterBase ToCharacter()
        {
            List<Skill> newSkillList = new List<Skill>();
            foreach (SkillAsset skillAsset in Skills.Skills)
            {
                List<Action> newActionList = new List<Action>();
                foreach (Action action in skillAsset.Actions.Actions)
                {
                    newActionList.Add(new Action(action.ActionType, action.Position - 1));
                }
                newSkillList.Add(new Skill(newActionList, skillAsset.Length));
            }

            return new CharacterBase(newSkillList);
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