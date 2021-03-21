using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.CoreUI;
using TimelineHero.Character;

namespace TimelineHero.Battle
{
    public class CharacterTimelineView : UiComponent
    {
        private List<CardWrapper> Cards;
        private List<Action> ActualBattleActions = new List<Action>();

        public int Length { get => Cards.Aggregate(0, (total, next) => total += next.Length); }
        public int MaxLength { get => maxLength; set => maxLength = value; }

        private int maxLength;


        private void Awake()
        {
            Cards = new List<CardWrapper>();
        }

        public bool TryAddCard(CardWrapper NewCard)
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

        public void RemoveCard(CardWrapper CardToRemove)
        {
            Cards.Remove(CardToRemove);
            CardToRemove.SetState(CardState.NoParent);
            RebuildPreBattleCards();
            ShrinkCards(true);
        }

        public Vector2 GetContentSize()
        {
            float x = Cards.Aggregate<CardWrapper, float>(0, (total, next) => total += next.Size.x);
            return new Vector2(x, Card.GetCardStaticHeight());
        }

        public bool IsPositionInsideBounds(Vector2 Position)
        {
            return (Position.x > WorldBounds.min.x) && (Position.x < WorldBounds.max.x) &&
                (Position.y > WorldBounds.min.y) && (Position.y < WorldBounds.max.y);
        }

        public Action GetActionInPosition(int Position)
        {
            if (Position >= ActualBattleActions.Count)
            {
                return new Action(CharacterActionType.Empty, Position, Cards.Last().GetSkill().Owner);
            }

            return ActualBattleActions[Position];
        }

        public bool IsEnoughSpaceForCard(CardWrapper NewCard)
        {
            return Length + NewCard.Length <= MaxLength;
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
            Cards.Clear();

            return newCards;
        }

        private void AddCardInternal(CardWrapper NewCard, bool SmoothMotion)
        {
            NewCard.SetParent(GetTransform());
            RebuildPreBattleCards();
            ShrinkCards(SmoothMotion);
        }

        private void RebuildPreBattleCards()
        {
            List<Skill> skills = GetSkills();

            for (int i = 0; i < Cards.Count; ++i)
            {
                if (!skills[i].NeedsPreBattleRebuild())
                {
                    Cards[i].SetState(CardState.BoardPrePlay, Cards[i].HandCard);
                    continue;
                }

                Skill newSkill = skills[i].Clone();

                if (newSkill.IsAdrenalineSkill())
                {
                    SkillUtils.RebuildAdrenalineSkill(skills, i, newSkill);
                }
                if (skills[i].IsKeyOutSkill())
                {
                    SkillUtils.RebuildKeyOutSkill(skills, i, newSkill);
                }

                newSkill.Initialize();

                Card newCard = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardPrefab);
                newCard.SetSkill(newSkill);
                Cards[i].SetState(CardState.BoardPrePlay, newCard);
            }
        }

        private void RebuildBattleCards()
        {
            List<Skill> skills = GetSkills();
            
            for (int i = 0; i < Cards.Count; ++i)
            {
                if (!skills[i].NeedsBattleRebuild())
                {
                    Cards[i].SetState(CardState.BoardPlay, Cards[i].HandCard);
                    continue;
                }

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

                newSkill.Initialize();

                Card newCard = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardPrefab);
                newCard.SetSkill(newSkill);
                Cards[i].SetState(CardState.BoardPlay, newCard);
            }
        }

        private void CreateActualActions()
        {
            ActualBattleActions.Clear();

            foreach (CardWrapper card in Cards)
            {
                Skill skill = card.BoardBattleCard.GetSkill();
                for (int i = 0; i < skill.VirtualLength; ++i)
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