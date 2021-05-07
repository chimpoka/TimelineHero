using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.Battle;
using TimelineHero.CoreUI;
using TimelineHero.Character;

namespace TimelineHero.BattleView
{
    public class CharacterTimelineView : UiComponent
    {
        public int Length { get => Cards.Aggregate(0, (total, next) => total += next.Length); }
        public int MaxLength { get => maxLength; set => maxLength = value; }

        protected List<CardWrapper> Cards = new List<CardWrapper>();
        private int maxLength;
        private CharacterTimeline CharacterTimelineCached;

        public void Initialize(CharacterTimeline CharacterTimelineRef)
        {
            CharacterTimelineCached = CharacterTimelineRef;
        }

        public bool TryAddCard(CardWrapper NewCard)
        {
            if (Cards.Contains(NewCard))
                return false;

            if (Cards.Count > 0 && SkillUtils.IsOpeningSkill(NewCard.GetOriginalSkill()))
                return false;

            if (SkillUtils.IsClosingSkill(Cards.LastOrDefault()?.GetOriginalSkill()))
                return false;

            AddCard(NewCard, true);

            if (Length > MaxLength)
            {
                RemoveCard(NewCard);
                return false;
            }

            return true;
        }

        public bool TryInsertCard(CardWrapper NewCard, int index)
        {
            if (index > Cards.Count)
                return false;

            if (Cards.Contains(NewCard))
                return false;

            if (index > 0 && SkillUtils.IsOpeningSkill(NewCard.GetOriginalSkill()))
                return false;

            if (index < Cards.Count && SkillUtils.IsClosingSkill(NewCard.GetOriginalSkill()))
                return false;

            if (index == 0 && SkillUtils.IsOpeningSkill(Cards.FirstOrDefault()?.GetOriginalSkill()))
                return false;

            if (index == Cards.Count && SkillUtils.IsClosingSkill(Cards.LastOrDefault()?.GetOriginalSkill()))
                return false;

            InsertCard(NewCard, index, true);

            if (Length > MaxLength)
            {
                RemoveCard(NewCard);
                return false;
            }

            return true;
        }

        public void AddCard(CardWrapper NewCard, bool SmoothMotion)
        {
            Cards.Add(NewCard);
            AddCardInternal(NewCard, SmoothMotion);
        }

        public void InsertCard(CardWrapper NewCard, int Index, bool SmoothMotion)
        {
            Cards.Insert(Index, NewCard);
            AddCardInternal(NewCard, SmoothMotion);
        }

        public void ReplaceWith(CardWrapper FromCard, CardWrapper ToCard)
        {
            int index = Cards.IndexOf(FromCard);
            Cards[index].DestroyUiObject();
            Cards[index] = ToCard;
            AddCardInternal(ToCard, true);
        }

        private void AddCardInternal(CardWrapper NewCard, bool SmoothMotion)
        {
            NewCard.SetParent(GetTransform());
            UpdateCardsLayout(SmoothMotion);
        }

        public void RemoveCard(CardWrapper CardToRemove)
        {
            Cards.Remove(CardToRemove);
            CardToRemove.SetState(CardState.Hand, CardToRemove.GetOriginalSkill());
            UpdateCardsLayout(true);

            UpdateTimelineData();
        }

        public void UpdateCardsLayout(bool SmoothMotion)
        {
            RebuildPreBattleCards();
            ShrinkCards(SmoothMotion);
        }

        public Vector2 GetContentSize()
        {
            float x = Cards.Aggregate<CardWrapper, float>(0, (total, next) => total += next.Size.x);
            return new Vector2(x, Card.GetCardStaticHeight());
        }

        public bool IsPositionInsideBounds(Vector2 Position)
        {
            Bounds bounds = WorldBounds;

            return (Position.x > bounds.min.x) && (Position.x < bounds.max.x) &&
                (Position.y > bounds.min.y) && (Position.y < bounds.max.y);
        }

        public void OnStartPlayState()
        {
            RebuildBattleCards();
            CharacterTimelineCached.CreateActualActions();
        }

        public List<Skill> GetOriginalSkills()
        {
            return Cards.Select(card => card.GetOriginalSkill()).ToList();
        }

        public List<Skill> GetSkills()
        {
            return Cards.Select(card => card.GetSkill()).ToList();
        }

        public List<CardWrapper> RemoveCardsFromTimeline()
        {
            List<CardWrapper> newCards = new List<CardWrapper>(Cards);
            foreach (CardWrapper card in newCards)
            {
                card.SetParent(GetTransform().parent);
            }
            Cards.Clear();

            UpdateTimelineData();

            return newCards;
        }

        public void RebuildPreBattleCards()
        {
            List<Skill> skills = GetOriginalSkills();

            for (int i = 0; i < Cards.Count; ++i)
            {
                Skill newSkill = skills[i].Clone();

                if (newSkill.IsAdrenalineSkill())
                {
                    SkillUtils.RebuildAdrenalineSkill(skills, i, newSkill);
                }
                if (newSkill.IsKeyOutSkill())
                {
                    SkillUtils.RebuildKeyOutSkill(skills, i, newSkill);
                }

                Cards[i].SetState(CardState.BoardPrePlay, newSkill);
            }

            UpdateTimelineData();
        }

        public void RebuildBattleCards()
        {
            List<Skill> skills = GetOriginalSkills();
            
            for (int i = 0; i < Cards.Count; ++i)
            {
                Skill newSkill = skills[i].Clone();

                if (newSkill.IsAdrenalineSkill())
                {
                    SkillUtils.RebuildAdrenalineSkill(skills, i, newSkill);
                }
                if (skills[i].IsKeyOutSkill())
                {
                    SkillUtils.RebuildKeyOutSkill(skills, i, newSkill);
                }
                if (skills[i].IsRandomSkill())
                {
                    SkillUtils.RebuildRandomActionSkill(newSkill);
                }
                if (skills[i].IsLuckSkill())
                {
                    SkillUtils.RebuildLuckSkill(newSkill);
                }

                Cards[i].SetState(CardState.BoardPlay, newSkill);
            }

            UpdateTimelineData();
        }

        private void ShrinkCards(bool SmoothMotion)
        {
            Vector2 cardPosition = Vector2.zero;

            foreach (CardWrapper card in Cards)
            {
                if (SmoothMotion)
                {
                    card.DOAnchorPos(cardPosition, 0.5f);
                }
                else
                {
                    card.AnchoredPosition = cardPosition;
                }
                cardPosition += new Vector2(card.Size.x, 0);
            }
        }

        private void UpdateTimelineData()
        {
            CharacterTimelineCached.SetSkills(GetSkills());
        }
    }
}