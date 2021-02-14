using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class SkillContainerView : MonoBehaviour
    {
        // Top, Bottom, Left, Right
        public Vector4 Border = new Vector4(20, 20, 20, 20);
        public Vector2 SkillOffset = new Vector2(10, 10);

        private RectTransform TransformCached;
        private List<SkillView> Skills;

        private void Awake()
        {
            TransformCached = GetComponent<RectTransform>();
            Skills = new List<SkillView>();
        }

        public void AddSkill(SkillView Skill)
        {
            RectTransform newSkillTransform = Skill.GetComponent<RectTransform>();
            newSkillTransform.SetParent(transform);
            newSkillTransform.localScale = Vector3.one;
            newSkillTransform.anchorMin = new Vector2(0, 0);
            newSkillTransform.anchorMax = new Vector2(0, 0);

            Vector2 newPosition = new Vector2(Border.x, Border.z);

            foreach(SkillView skill in Skills)
            {
                Vector2 size = skill.GetSize();
                newPosition += new Vector2(skill.GetSize().x + SkillOffset.x, 0);
            }

            newSkillTransform.anchoredPosition = newPosition;

            Skills.Add(Skill);
        }
    }
}