using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;


namespace TimelineHero.Battle
{
    public class BattleSkillController
    {
        SkillContainerView SkillContainerCached;
        CharacterTimelineView AlliedTimelineCached;
        public List<CharacterBase> AlliedCharacters;

        public void SpawnSkills()
        {
            foreach(CharacterBase character in AlliedCharacters)
            {
                foreach (Skill skill in character.Skills)
                {
                    SkillView skillView = MonoBehaviour.Instantiate(BattlePrefabsScriptableObject.Instance.SkillPrefab);
                    skillView.SetSkill(skill);
                    SkillContainerCached.AddSkill(skillView);
                }
            }
        }

        public void SetAlliedCharacters(List<CharacterBase> NewAlliedCharacters)
        {
            AlliedCharacters = NewAlliedCharacters;
        }

        public void SetSkillContainer(SkillContainerView NewSkillContainer)
        {
            SkillContainerCached = NewSkillContainer;
        }
    }
}