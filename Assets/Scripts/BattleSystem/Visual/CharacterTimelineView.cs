﻿using System.Collections.Generic;
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
                AddSkill(NewSkill);
                return true;
            }

            return false;
        }

        public void AddSkill(SkillView NewSkill)
        {
            NewSkill.SetParent(GetTransform());
            NewSkill.AnchoredPosition = new Vector2(GetContentSize().x, 0);
            NewSkill.LocationType = SkillLocationType.Timeline;

            Skills.Add(NewSkill);
        }

        public void RemoveSkill(SkillView SkillToRemove)
        {
            Skills.Remove(SkillToRemove);
            SkillToRemove.LocationType = SkillLocationType.NoParent;

            ShrinkSkills();
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

        private void ShrinkSkills()
        {
            Vector2 skillPosition = Vector2.zero;

            foreach (SkillView skill in Skills)
            {
                skill.AnchoredPosition = skillPosition;
                skillPosition += new Vector2(skill.Size.x, 0);
            }
        }
    }
}