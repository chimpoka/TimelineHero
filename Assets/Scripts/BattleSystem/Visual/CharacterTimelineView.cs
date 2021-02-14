using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class CharacterTimelineView : MonoBehaviour
    {
        List<SkillView> Skills;

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
            skillTransform.anchoredPosition = new Vector2(GetSize().x, 0);

            Skills.Add(NewSkill);

            UpdateSize();
        }

        public Vector2 GetSize()
        {
            Vector2 size = Vector2.zero;

            foreach (SkillView skill in Skills)
            {
                Vector2 skillSize = skill.GetSize();
                size.x += skillSize.x;
                size.y = size.y > skillSize.y ? size.y : skillSize.y;
            }

            return size;
        }

        public void UpdateSize()
        {
            RectTransform timelineTransform = GetComponent<RectTransform>();
            timelineTransform.sizeDelta = GetSize();
        }
    }
}