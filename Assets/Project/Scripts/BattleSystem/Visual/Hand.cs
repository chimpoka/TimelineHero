using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.Battle
{
    public class Hand : UiComponent
    {
        // Top, Bottom, Left, Right
        public Vector4 Border = new Vector4(20, 20, 20, 20);
        public Vector2 SkillOffset = new Vector2(10, 10);

        private List<Card> Skills;

        public void AddCard(Card Skill)
        {
            Skills = Skills ?? new List<Card>();

            Skills.Add(Skill);
            Skill.SetParent(GetTransform());
            Skill.LocationType = CardLocationType.Hand;
            ShrinkSkills();
        }

        public void RemoveCard(Card Skill)
        {
            Skills.Remove(Skill);
            Skill.LocationType = CardLocationType.NoParent;
            ShrinkSkills();
        }

        private void ShrinkSkills()
        {
            Vector2 newPosition = new Vector2(Border.y, Border.z);

            foreach (Card skill in Skills)
            {
                if (newPosition.x + skill.Size.x > Size.x)
                {
                    newPosition.x = Border.z;
                    newPosition.y += Card.GetCardStaticHeight() + SkillOffset.y;
                }

                skill.DOAnchorPos(newPosition);
                
                newPosition += new Vector2(skill.Size.x + SkillOffset.x, 0);
            }
        }
    }
}