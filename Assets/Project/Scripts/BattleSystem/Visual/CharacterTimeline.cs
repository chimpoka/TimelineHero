using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.CoreUI;
using TimelineHero.Character;
using DG.Tweening;

namespace TimelineHero.Battle
{
    public class CharacterTimeline : UiComponent
    {
        public int Length { get => Cards.Aggregate(0, (total, next) => total += next.Length); }
        public int MaxLength { get => maxLength; set => maxLength = value; }

        public System.Action OnLengthChanged;

        protected List<CardWrapper> Cards = new List<CardWrapper>();
        private List<Action> ActualBattleActions = new List<Action>();
        private int maxLength;

        public bool TryAddCard(CardWrapper NewCard)
        {
            if (Cards.Contains(NewCard))
                return false;

            if (Cards.Count > 0 && SkillUtils.IsOpeningSkill(NewCard.GetSkill()))
                return false;

            if (SkillUtils.IsClosingSkill(Cards.LastOrDefault()?.GetSkill()))
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

            if (index > 0 && SkillUtils.IsOpeningSkill(NewCard.GetSkill()))
                return false;

            if (index < Cards.Count && SkillUtils.IsClosingSkill(NewCard.GetSkill()))
                return false;

            if (index == 0 && SkillUtils.IsOpeningSkill(Cards.FirstOrDefault()?.GetSkill()))
                return false;

            if (index == Cards.Count && SkillUtils.IsClosingSkill(Cards.LastOrDefault()?.GetSkill()))
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
            CardToRemove.SetState(CardState.Hand, CardToRemove.HandCard.GetSkill());
            UpdateCardsLayout(true);
        }

        public void UpdateCardsLayout(bool SmoothMotion)
        {
            RebuildPreBattleCards();
            ShrinkCards(SmoothMotion);

            OnLengthChanged?.Invoke();
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

        public Action GetActionInPosition(int Position)
        {
            if (Position >= ActualBattleActions.Count)
            {
                return new Action(CharacterActionType.Empty, Position, Cards.Last().GetSkill().Owner);
            }

            return ActualBattleActions[Position];
        }

        public void OnConstruct()
        {
            OnLengthChanged?.Invoke();
        }

        public void OnStartPlayState()
        {
            RebuildBattleCards();
            CreateActualActions();
        }

        private List<Skill> GetSkills()
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

            return newCards;
        }

        public void RebuildPreBattleCards()
        {
            List<Skill> skills = GetSkills();

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
        }

        public void RebuildBattleCards()
        {
            List<Skill> skills = GetSkills();
            
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
        }

        private void CreateActualActions()
        {
            ActualBattleActions.Clear();

            foreach (CardWrapper card in Cards)
            {
                Skill skill = card.BoardBattleCard.GetSkill();
                for (int i = 0; i < skill.Length; ++i)
                {
                    ActualBattleActions.Add(skill.GetActionInPosition(i));
                }
            }
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
    }
}