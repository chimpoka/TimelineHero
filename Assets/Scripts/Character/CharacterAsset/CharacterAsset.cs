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
            List<Skill> skillList = new List<Skill>();
            foreach (SkillAsset skillAsset in Skills.Skills)
            {
                skillList.Add(new Skill(skillAsset.Actions.Actions));
            }

            return new CharacterBase(skillList);
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