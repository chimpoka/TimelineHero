using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
using UnityEngine.EventSystems;

namespace TimelineHero.Battle
{


    public class BattleSkillController
    {
        public float CanvasScaleFactor;

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

                    skillView.RelationData.Parent = SkillParentObject.Container;
                    skillView.RelationData.PositionInContainer = skillView.AnchoredPosition;

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
            // Move to foreground
            Skill.GetTransform().SetSiblingIndex(Skill.GetTransform().parent.childCount - 1);
        }

        public void OnSkillPointerUp(SkillView Skill, PointerEventData eventData)
        {
            // Temp shit!
            if (AlliedTimelineCached.IsPositionInsideBounds(Skill.WorldBounds.center))
            {
                if (Skill.RelationData.Parent == SkillParentObject.Container)
                {
                    AlliedTimelineCached.AddSkill(Skill);
                    Skill.RelationData.PositionInTimeline = Skill.AnchoredPosition;
                    Skill.RelationData.Parent = SkillParentObject.Timeline;
                }
                else if (Skill.RelationData.Parent == SkillParentObject.Timeline)
                {
                    Skill.AnchoredPosition = Skill.RelationData.PositionInTimeline;
                }
            }
            else
            {
                AlliedTimelineCached.RemoveSkill(Skill);
                Skill.GetTransform().SetParent(SkillContainerCached.GetTransform());
                Skill.AnchoredPosition = Skill.RelationData.PositionInContainer;
                Skill.RelationData.Parent = SkillParentObject.Container;
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
            Skill.AnchoredPosition += eventData.delta / CanvasScaleFactor;
        }
        #endregion SkillEvents
    }
}