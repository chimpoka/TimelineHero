using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.CoreUI;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class CharacterTimelineView : UiComponent
    {
        private List<SkillView> Skills;

        public int Length { get => Skills.Aggregate(0, (total, next) => total += next.Length); }
        public int MaxLength { get => maxLength; set => maxLength = value; }

        private int maxLength;


        private void Awake()
        {
            Skills = new List<SkillView>();
        }

        public bool TryAddSkill(SkillView NewSkill)
        {
            if (SkillUtils.IsOpeningSkill(NewSkill.GetSkill()) && Skills.Count > 0)
                return false;

            if (SkillUtils.IsClosingSkill(Skills.LastOrDefault()?.GetSkill()))
                return false;

            if (!IsEnoughSpaceForSkill(NewSkill))
                return false;

            AddSkill(NewSkill, true);
            return true;
        }

        public void AddSkill(SkillView NewSkill, bool SmoothMotion)
        {
            Skills.Add(NewSkill);
            AddSkillInternal(NewSkill, SmoothMotion);
        }

        public void InsertSkill(SkillView NewSkill, int Index, bool SmoothMotion)
        {
            Skills.Insert(Index, NewSkill);
            AddSkillInternal(NewSkill, SmoothMotion);
        }

        public void RemoveSkill(SkillView SkillToRemove)
        {
            Skills.Remove(SkillToRemove);
            SkillToRemove.LocationType = SkillLocationType.NoParent;

            ShrinkSkills(true);
        }

        public Vector2 GetContentSize()
        {
            float x = Skills.Aggregate<SkillView, float>(0, (total, next) => total += next.Size.x);
            return new Vector2(x, SkillView.GetSkillStaticHeight());
        }

        public bool IsPositionInsideBounds(Vector2 Position)
        {
            return (Position.x > WorldBounds.min.x) && (Position.x < WorldBounds.max.x) &&
                (Position.y > WorldBounds.min.y) && (Position.y < WorldBounds.max.y);
        }

        public Action GetActionInPosition(int Position)
        {
            int prevSkillsLength = 0;

            foreach (SkillView skillView in Skills)
            {
                Skill skill = skillView.GetSkill();

                if (Position >= skill.Length + prevSkillsLength)
                {
                    prevSkillsLength += skill.Length;
                    continue;
                }

                return skill.GetActionInPosition(Position - prevSkillsLength);
            }

            return new Action(CharacterActionType.Empty, Position, Skills.Last().GetSkill().Owner);
        }

        public bool IsEnoughSpaceForSkill(SkillView NewSkill)
        {
            return Length + NewSkill.Length <= MaxLength;
        }

        public void RebuildSkillsForPlayState()
        {
            List<Skill> skills = GetSkills();

            for (int i = 0; i < skills.Count; ++i)
            {
                if (skills[i].IsRandomSkill())
                {
                    RebuildRandomActionSkill(skills[i], i);
                }
                if (skills[i].IsAdrenalineSkill())
                {
                    RebuildAdrenalineSkill(skills, i);
                }
            }
        }

        private void RebuildRandomActionSkill(Skill SkillRef, int Index)
        {
            SkillView NewSkill = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
            NewSkill.SetSkill(SkillUtils.GetRebuiltSkillWithRandomActions(SkillRef));
            ReplaceSkillAtIndex(Index, NewSkill);
        }

        private void RebuildAdrenalineSkill(List<Skill> SkillList, int Index)
        {
            SkillView NewSkill = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
            NewSkill.SetSkill(SkillUtils.GetRebuiltSkillWithAdrenaline(SkillList, Index));
            ReplaceSkillAtIndex(Index, NewSkill);
        }

        private List<Skill> GetSkills()
        {
            List<Skill> skills = new List<Skill>();

            for (int i = 0; i < Skills.Count; ++i)
            {
                skills.Add(Skills[i].GetSkill());
            }

            return skills;
        }

        private void ReplaceSkillAtIndex(int Index, SkillView NewSkill)
        {
            Skills[Index].DestroyGameObject();
            Skills.RemoveAt(Index);

            InsertSkill(NewSkill, Index, false);
        }

        public void ClearTimeline()
        {
            while (Skills.Count != 0)
            {
                Destroy(Skills[0].gameObject);
                Skills.RemoveAt(0);
            }
        }

        private void AddSkillInternal(SkillView NewSkill, bool SmoothMotion)
        {
            NewSkill.SetParent(GetTransform());
            NewSkill.LocationType = SkillLocationType.Timeline;

            ShrinkSkills(SmoothMotion);
        }

        private void ShrinkSkills(bool SmoothMotion)
        {
            Vector2 skillPosition = Vector2.zero;

            foreach (SkillView skill in Skills)
            {
                if (SmoothMotion)
                {
                    skill.DOAnchorPos(skillPosition);
                }
                else
                {
                    skill.AnchoredPosition = skillPosition;
                }
                skillPosition += new Vector2(skill.Size.x, 0);
            }
        }
    }
}