using System.Collections;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TimelineHero.Battle
{
    public enum CardState { Hand, BoardPrePlay, BoardPlay }

    public class CardWrapper : UiComponent, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public int Length { get => GetLength(); }

        public Card HandCard;
        public Card BoardPreBattleCard;
        public Card BoardBattleCard;

        public CardState State;

        public delegate void CardEventHandler(CardWrapper card, PointerEventData eventData);
        public event CardEventHandler OnPointerDownEvent;
        public event CardEventHandler OnPointerUpEvent;
        public event CardEventHandler OnBeginDragEvent;
        public event CardEventHandler OnEndDragEvent;
        public event CardEventHandler OnDragEvent;

        public void SetState(CardState NewState, Card NewCard)
        {
            NewCard.SetParent(GetTransform());
            Size = NewCard.Size;
            NewCard.gameObject.SetActive(true);
            NewCard.AnchoredPosition = Vector2.zero;

            if (NewState == CardState.Hand)
            {
                ClearCards();
                HandCard = NewCard;
            }
            else if (NewState == CardState.BoardPrePlay)
            {
                ClearCards();
                BoardPreBattleCard = NewCard;
            }
            else if (NewState == CardState.BoardPlay)
            {
                BoardBattleCard = NewCard;
                // Move to background
                BoardBattleCard.GetTransform().SetSiblingIndex(1);
                SkillUtils.CopyCardStats(BoardBattleCard, BoardPreBattleCard);
                BoardPreBattleCard.PlayAnimation();
            }

            State = NewState;
        }

        public int GetLength()
        {
            if (State == CardState.Hand)
            {
                return HandCard.Length;
            }
            if (State == CardState.BoardPrePlay)
            {
                return BoardPreBattleCard.Length;
            }
            if (State == CardState.BoardPlay)
            {
                return BoardBattleCard.Length;
            }
            return 0;
        }

        private void ClearCard(Card CardRef)
        {
            if (CardRef != null && CardRef != HandCard)
            {
                if (CardRef != HandCard)
                {
                    CardRef.DestroyUiObject();
                }
                else
                {
                    CardRef = null;
                }
            }
        }

        private void ClearCards()
        {
            ClearCard(BoardPreBattleCard);
            ClearCard(BoardBattleCard);
        }

        public Skill GetSkill()
        {
            return HandCard.GetSkill();
        }

        #region SkillEvents
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(this, eventData);
        }
        #endregion SkillEvents
    }
}