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
            if (IsEnoughSpaceForSkill(NewSkill))
            {
                AddSkill(NewSkill, true);
                return true;
            }

            return false;
        }

        public void AddSkill(SkillView NewSkill, bool SmoothMotion, int Index = -1)
        {
            if (Index == -1)
            {
                Skills.Add(NewSkill);
            }
            else if (Index >= 0)
            {
                Skills.Insert(Index, NewSkill);
            }

            NewSkill.SetParent(GetTransform());
            NewSkill.LocationType = SkillLocationType.Timeline;

            ShrinkSkills(SmoothMotion);
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

        public void RebuildSkillsWithRandomActions()
        {
            Dictionary<int, SkillView> skillsWithRandomActions = new Dictionary<int, SkillView>();

            for (int i = 0; i < Skills.Count; ++i)
            {
                Skill skill = Skills[i].GetSkill();

                if (skill.RandomActionsCounter > 0)
                {
                    SkillView NewSkill = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
                    NewSkill.SetSkill(SkillUtils.GetRebuiltSkillWithRandomActions(skill));
                    Skills[i].DestroyGameObject();
                    skillsWithRandomActions.Add(i, NewSkill);
                }
            }

            foreach (var skill in skillsWithRandomActions)
            {
                Skills.RemoveAt(skill.Key);
                AddSkill(skill.Value, false, skill.Key);
            }
        }

        public void ClearTimeline()
        {
            while (Skills.Count != 0)
            {
                Destroy(Skills[0].gameObject);
                Skills.RemoveAt(0);
            }
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