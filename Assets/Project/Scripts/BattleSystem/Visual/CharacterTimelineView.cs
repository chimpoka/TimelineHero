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
            if (Cards.Contains(NewCard))
                return false;

            if (SkillUtils.IsOpeningSkill(NewCard.GetSkill()) && Cards.Count > 0)
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
            if (Cards.Contains(NewCard))
                return false;

            InsertCard(NewCard, index, true);

            return true;
        }

        public bool TryInsertVisibleCard(CardWrapper NewCard)
        {
            CardWrapper invisibleCard = Cards.Find(card => card.gameObject.activeInHierarchy == false);
            int index = Cards.IndexOf(invisibleCard);
            TryRemoveCard(invisibleCard);
            TryInsertCard(NewCard, index);

            return true;
        }

        public bool TryInsertInvisibleCard(CardWrapper NewCard)
        {
            CardWrapper invisibleCard = Cards.Find(card => card.gameObject.activeInHierarchy == false);
            if (!invisibleCard)
            {
                invisibleCard = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardWrapperPrefab);
                Card cardCopy = MonoBehaviour.Instantiate(BattlePrefabsConfig.Instance.CardPrefab);
                cardCopy.SetSkill(NewCard.GetSkill());
                invisibleCard.SetState(CardState.Hand, cardCopy);
                invisibleCard.gameObject.SetActive(false);
            }

            if (Cards.Count == 0)
            {
                TryAddCard(invisibleCard);
                return true;
            }

            float newCardPos = NewCard.WorldBounds.min.x;
            float cardEndPos = WorldBounds.min.x;

            float timelineLenght = Cards.Aggregate(0.0f, (total, next) => total += next != invisibleCard ? next.WorldBounds.size.x : 0.0f);
            if (newCardPos > cardEndPos + timelineLenght)
            {
                TryRemoveCard(invisibleCard);
                TryAddCard(invisibleCard);
                return true;
            }

            int i = 0;
            foreach (CardWrapper card in Cards)
            {
                
                if (card == invisibleCard)
                {
                    continue;
                }

                float cardStartPos = cardEndPos;
                cardEndPos += card.WorldBounds.size.x;
                if (newCardPos < cardEndPos)
                {
                    int index;
                    if (newCardPos < cardStartPos + (cardEndPos - cardStartPos) / 2)
                    {
                        index = i;
                    }
                    else
                    {
                        index = i + 1;
                    }
                    print(index);
                    if (!Cards.Find(x => x.gameObject.activeInHierarchy == false))
                    {
                        TryInsertCard(invisibleCard, index);
                    }
                    else if (Cards.IndexOf(invisibleCard) != index)
                    {
                        if (index >= Cards.Count)
                        {
                            TryRemoveCard(invisibleCard);
                            TryAddCard(invisibleCard);
                        }
                        else
                        {
                            TryRemoveCard(invisibleCard);
                            TryInsertCard(invisibleCard, index);
                        }
                    }

                    return true;
                }

                i++;
            }

            return false;
        }

        public bool TryRemoveInvisibleCard()
        {
            CardWrapper invisibleCard = Cards.Find(card => card.gameObject.activeInHierarchy == false);
            return TryRemoveCard(invisibleCard);
        }

        public bool TryRemoveCard(CardWrapper NewCard)
        {
            if (Cards.Contains(NewCard))
            {
                RemoveCard(NewCard);
                return true;
            }

            return false;
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