using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;
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

        void Start()
        {

        }

        void Update()
        {

        }

        public void AddSkill(SkillView NewSkill)
        {
            RectTransform skillTransform = NewSkill.GetComponent<RectTransform>();
            skillTransform.SetParent(transform);
            skillTransform.localScale = Vector2.one;
            skillTransform.anchorMin = Vector2.zero;
            skillTransform.anchorMax = Vector2.zero;
            skillTransform.anchoredPosition = new Vector2(GetContentSize().x, 0);

            Skills.Add(NewSkill);
        }

        public void RemoveSkill(SkillView SkillToRemove)
        {
            //Skills.Remove(SkillToRemove);
            ShrinkSkills();
        }

        public Vector2 GetContentSize()
        {
            Vector2 size = Vector2.zero;

            foreach (SkillView skill in Skills)
            {
                size.x += skill.Size.x;
                size.y = size.y > skill.Size.y ? size.y : skill.Size.y;
            }

            return size;
        }

        public void UpdateSize()
        {
            GetTransform().sizeDelta = GetContentSize();
        }

        public void SetSize(Vector2 NewSize)
        {
            GetTransform().sizeDelta = NewSize;
        }

        public bool IsPositionInsideBounds(Vector2 Position)
        {
            print("Point: " + Position);
            print("Bounds min: " + WorldBounds.min);
            print("Bounds max: " + WorldBounds.max);

            return (Position.x > WorldBounds.min.x) && (Position.x < WorldBounds.max.x) &&
                (Position.y > WorldBounds.min.y) && (Position.y < WorldBounds.max.y);
        }

        private void ShrinkSkills()
        {

        }
    }
}