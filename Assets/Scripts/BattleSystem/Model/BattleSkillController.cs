using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
using UnityEngine.EventSystems;
using TimelineHero.Core;

namespace TimelineHero.Battle
{
    public class BattleSkillController
    {
        private List<CharacterBase> AlliedCharacters;
        private SkillContainerView SkillContainerCached;
        private CharacterTimelineView AlliedTimelineCached;

        public void SpawnSkills()
        {
            foreach(CharacterBase character in AlliedCharacters)
            {
                foreach (Skill skill in character.Skills)
                {
                    SkillView skillView = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
                    skillView.SetSkill(skill);
                    SkillContainerCached.AddSkill(skillView);

                    skillView.LocationType = SkillLocationType.Container;

                    skillView.OnPointerDownEvent += OnSkillPointerDown;
                    skillView.OnPointerUpEvent += OnSkillPointerUp;
                    skillView.OnBeginDragEvent += OnSkillBeginDrag;
                    //skillView.OnEndDragEvent += OnSkillEndDrag;
                    skillView.OnDragEvent += OnSkillDrag;
                }
            }
        }

        public void SetAlliedCharacters(List<CharacterBase> NewAlliedCharacters)
        {
            AlliedCharacters = NewAlliedCharacters;
        }

        public void SetAlliedTimeline(CharacterTimelineView NewAlliedTimeline)
        {
            AlliedTimelineCached = NewAlliedTimeline;
        }

        public void SetSkillContainer(SkillContainerView NewSkillContainer)
        {
            SkillContainerCached = NewSkillContainer;
        }

        #region SkillEvents
        public void OnSkillPointerDown(SkillView Skill, PointerEventData eventData)
        {
            Skill.AnchoredPosition += new Vector2(-6, 6);

            if (Skill.LocationType == SkillLocationType.Container)
            {
                SkillContainerCached.RemoveSkill(Skill);
            }
            else if (Skill.LocationType == SkillLocationType.Timeline)
            {
                AlliedTimelineCached.RemoveSkill(Skill);
            }

            // Move to foreground
            Skill.GetTransform().SetSiblingIndex(Skill.GetTransform().parent.childCount - 1);
        }

        public void OnSkillPointerUp(SkillView Skill, PointerEventData eventData)
        {
            if (AlliedTimelineCached.IsPositionInsideBounds(Skill.WorldBounds.center))
            {
                if (!AlliedTimelineCached.TryAddSkill(Skill))
                {
                    SkillContainerCached.AddSkill(Skill);
                }
            }
            else
            {
                SkillContainerCached.AddSkill(Skill);
            }
        }

        public void OnSkillBeginDrag(SkillView Skill, PointerEventData eventData)
        {

        }

        public void OnSkillEndDrag(SkillView Skill, PointerEventData eventData)
        {

        }

        public void OnSkillDrag(SkillView Skill, PointerEventData eventData)
        {
            Skill.AnchoredPosition += eventData.delta / GameInstance.Instance.CanvasScaleFactor;
        }
        #endregion SkillEvents
    }
}