using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimelineHero.CoreUI;
using TimelineHero.Character;
using DG.Tweening;

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

#region GhostCardInsertion
        public bool TryInsertInvisibleCard(CardWrapper NewCard)
        {
            if (Cards.Count == 0)
            {
                return TryAddCard(CreateInvisibleCard(NewCard));
            }

            float newCardStartPosition = NewCard.WorldBounds.min.x;
            float previousCardEndPosition = WorldBounds.min.x;
            float previousCardHandMinusPreBattleSize = 0;

            CardWrapper invisibleCard = GetInvisibleCard();
            int i = 0;
            foreach (CardWrapper card in Cards)
            {
                if (card == invisibleCard)
                {
                    previousCardEndPosition += previousCardHandMinusPreBattleSize;
                    continue;
                }

                float currentCardStartPosition = previousCardEndPosition;
                float currentHandCardEndPosition = currentCardStartPosition + card.HandCard.WorldBounds.size.x;
                float currentHandCardCenterPosition = currentCardStartPosition + card.HandCard.WorldBounds.extents.x;

                if (newCardStartPosition < currentHandCardEndPosition)
                {
                    int index = newCardStartPosition < currentHandCardCenterPosition ? i : i + 1;

                    invisibleCard = invisibleCard ?? CreateInvisibleCard(NewCard);
                    TryRemoveCard(invisibleCard);
                    if (!TryInsertCard(invisibleCard, index))
                    {
                        invisibleCard.DestroyUiObject();
                        return false;
                    }

                    return true;
                }

                previousCardEndPosition += card.WorldBounds.size.x;
                previousCardHandMinusPreBattleSize = card.HandCard.WorldBounds.size.x - card.WorldBounds.size.x;

                i++;
            }

            return false;
        }

        public bool TryInsertVisibleCard(CardWrapper NewCard)
        {
            CardWrapper invisibleCard = GetInvisibleCard();

            if (invisibleCard == null)
            {
                return TryAddCard(NewCard);
            }

            int index = Cards.IndexOf(invisibleCard);
            TryRemoveInvisibleCard();
            return TryInsertCard(NewCard, index);
        }

        CardWrapper GetInvisibleCard()
        {
            return Cards.Find(card => card.gameObject.activeInHierarchy == false);
        }

        private CardWrapper CreateInvisibleCard(CardWrapper FromCard)
        {
            CardWrapper invisibleCard = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardWrapperPrefab);
            Card cardCopy = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardPrefab);
            cardCopy.SetSkill(FromCard.GetSkill());
            invisibleCard.SetState(CardState.Hand, cardCopy);
            invisibleCard.gameObject.SetActive(false);

            return invisibleCard;
        }

        public bool TryRemoveInvisibleCard()
        {
            CardWrapper invisibleCard = GetInvisibleCard();

            if (invisibleCard == null)
                return false;

            RemoveCard(invisibleCard);
            invisibleCard.DestroyUiObject();

            return true;
        }
#endregion GhostCardInsertion

        public bool TryRemoveCard(CardWrapper NewCard)
        {
            if (!Cards.Contains(NewCard))
                return false;

            RemoveCard(NewCard);
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
            CardToRemove.SetState(CardState.Hand, CardToRemove.HandCard);
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
                    card.DOAnchorPos(cardPosition, 0.5f).SetEase(Ease.Unset);
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