using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.CoreUI;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class CharacterTimelineView : UiComponent
    {
        private List<Card> Cards;

        public int Length { get => Cards.Aggregate(0, (total, next) => total += next.Length); }
        public int MaxLength { get => maxLength; set => maxLength = value; }

        private int maxLength;


        private void Awake()
        {
            Cards = new List<Card>();
        }

        public bool TryAddCard(Card NewCard)
        {
            if (SkillUtils.IsOpeningSkill(NewCard.GetSkill()) && Cards.Count > 0)
                return false;

            if (SkillUtils.IsClosingSkill(Cards.LastOrDefault()?.GetSkill()))
                return false;

            if (!IsEnoughSpaceForCard(NewCard))
                return false;

            AddCard(NewCard, true);
            return true;
        }

        public void AddCard(Card NewCard, bool SmoothMotion)
        {
            Cards.Add(NewCard);
            AddCardInternal(NewCard, SmoothMotion);
        }

        public void InsertCard(Card NewCard, int Index, bool SmoothMotion)
        {
            Cards.Insert(Index, NewCard);
            AddCardInternal(NewCard, SmoothMotion);
        }

        public void RemoveCard(Card CardToRemove)
        {
            Cards.Remove(CardToRemove);
            CardToRemove.LocationType = CardLocationType.NoParent;

            ShrinkCards(true);
        }

        public Vector2 GetContentSize()
        {
            float x = Cards.Aggregate<Card, float>(0, (total, next) => total += next.Size.x);
            return new Vector2(x, Card.GetCardStaticHeight());
        }

        public bool IsPositionInsideBounds(Vector2 Position)
        {
            return (Position.x > WorldBounds.min.x) && (Position.x < WorldBounds.max.x) &&
                (Position.y > WorldBounds.min.y) && (Position.y < WorldBounds.max.y);
        }

        public Action GetActionInPosition(int Position)
        {
            int prevSkillsLength = 0;

            foreach (Card card in Cards)
            {
                Skill skill = card.GetSkill();

                if (Position >= skill.Length + prevSkillsLength)
                {
                    prevSkillsLength += skill.Length;
                    continue;
                }

                return skill.GetActionInPosition(Position - prevSkillsLength);
            }

            return new Action(CharacterActionType.Empty, Position, Cards.Last().GetSkill().Owner);
        }

        public bool IsEnoughSpaceForCard(Card NewCard)
        {
            return Length + NewCard.Length <= MaxLength;
        }

        public void RebuildCardsForPlayState()
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
            Card NewSkill = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
            NewSkill.SetSkill(SkillUtils.GetRebuiltSkillWithRandomActions(SkillRef));
            ReplaceCardAtIndex(Index, NewSkill);
        }

        private void RebuildAdrenalineSkill(List<Skill> SkillList, int Index)
        {
            Card NewSkill = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.SkillPrefab);
            NewSkill.SetSkill(SkillUtils.GetRebuiltSkillWithAdrenaline(SkillList, Index));
            ReplaceCardAtIndex(Index, NewSkill);
        }

        private List<Skill> GetSkills()
        {
            List<Skill> skills = new List<Skill>();

            for (int i = 0; i < Cards.Count; ++i)
            {
                skills.Add(Cards[i].GetSkill());
            }

            return skills;
        }

        private void ReplaceCardAtIndex(int Index, Card NewCard)
        {
            Cards[Index].DestroyGameObject();
            Cards.RemoveAt(Index);

            InsertCard(NewCard, Index, false);
        }

        public List<Card> RemoveCardsFromTimeline()
        {
            List<Card> newCards = new List<Card>(Cards);
            Cards.Clear();

            return newCards;
        }

        private void AddCardInternal(Card NewCard, bool SmoothMotion)
        {
            NewCard.SetParent(GetTransform());
            NewCard.LocationType = CardLocationType.Board;

            ShrinkCards(SmoothMotion);
        }

        private void ShrinkCards(bool SmoothMotion)
        {
            Vector2 cardPosition = Vector2.zero;

            foreach (Card card in Cards)
            {
                if (SmoothMotion)
                {
                    card.DOAnchorPos(cardPosition);
                }
                else
                {
                    card.AnchoredPosition = cardPosition;
                }
                cardPosition += new Vector2(card.Size.x, 0);
            }
        }
    }
}