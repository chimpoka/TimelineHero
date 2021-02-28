using System.Collections;
using System.Collections.Generic;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class SkillContainerView : UiComponent
    {
        // Top, Bottom, Left, Right
        public Vector4 Border = new Vector4(20, 20, 20, 20);
        public Vector2 SkillOffset = new Vector2(10, 10);

        private List<SkillView> Skills;

        public void AddSkill(SkillView Skill)
        {
            Skills = Skills ?? new List<SkillView>();

            Skills.Add(Skill);
            Skill.SetParent(GetTransform());
            Skill.LocationType = SkillLocationType.Container;
            ShrinkSkills();
        }

        public void RemoveSkill(SkillView Skill)
        {
            Skills.Remove(Skill);
            Skill.LocationType = SkillLocationType.NoParent;
            ShrinkSkills();
        }

        private void ShrinkSkills()
        {
            Vector2 newPosition = new Vector2(Border.y, Border.z);

            foreach (SkillView skill in Skills)
            {
                if (newPosition.x + skill.Size.x > Size.x)
                {
                    newPosition.x = Border.z;
                    newPosition.y += SkillView.GetSkillStaticHeight() + SkillOffset.y;
                }

                skill.AnchoredPosition = newPosition;
                
                newPosition += new Vector2(skill.Size.x + SkillOffset.x, 0);
            }
        }
    }
}