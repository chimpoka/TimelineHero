using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.CoreUI;

namespace TimelineHero.Battle
{
    public class CharacterTimelineView : UiComponent
    {
        private List<SkillView> Skills;

        private void Awake()
        {
            Skills = new List<SkillView>();
        }

        public void AddSkill(SkillView NewSkill)
        {
            NewSkill.SetParent(GetTransform());
            NewSkill.AnchoredPosition = new Vector2(GetContentSize().x, 0);
            
            Skills.Add(NewSkill);
        }

        public void RemoveSkill(SkillView SkillToRemove)
        {
            Skills.Remove(SkillToRemove);

            ShrinkSkills();
        }

        public Vector2 GetContentSize()
        {
            float x = Skills.Aggregate<SkillView, float>(0, (total, next) => total += next.Size.x);
            return new Vector2(x, SkillView.GetSkillStaticHeight());

            //Vector2 contentSize = new Vector2(0, SkillView.GetSkillStaticHeight());

            //foreach (SkillView skill in Skills)
            //{
            //    contentSize.x += skill.Size.x;
            //}

            //return contentSize;
        }

        public bool IsPositionInsideBounds(Vector2 Position)
        {
            return (Position.x > WorldBounds.min.x) && (Position.x < WorldBounds.max.x) &&
                (Position.y > WorldBounds.min.y) && (Position.y < WorldBounds.max.y);
        }

        public int GetLength()
        {
            return Skills.Aggregate(0, (total, next) => total += next.GetLength());

            //int length = 0;

            //foreach (SkillView skill in Skills)
            //{
            //    length += skill.GetLength();
            //}

            //return length;
        }

        private void ShrinkSkills()
        {
            Vector2 skillPosition = Vector2.zero;

            foreach (SkillView skill in Skills)
            {
                skill.AnchoredPosition = skillPosition;
                skill.RelationData.PositionInTimeline = skillPosition;
                skillPosition += new Vector2(skill.Size.x, 0);
            }
        }
    }
}