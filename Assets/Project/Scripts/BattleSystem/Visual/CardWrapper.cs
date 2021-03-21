using System.Collections;
using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TimelineHero.Battle
{
    public enum CardState { NoParent, Hand, BoardPrePlay, BoardPlay }

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

        public void SetState(CardState NewState, Card NewCard = null)
        {
            if (NewState == CardState.NoParent)
            {
                SetHandCard(NewCard);
            }
            else if (NewState == CardState.Hand)
            {
                SetHandCard(NewCard);
            }
            else if (NewState == CardState.BoardPrePlay)
            {
                SetBoardPreBattleCard(NewCard);
            }
            else if (NewState == CardState.BoardPlay)
            {
                SetBoardBattleCard(NewCard);
            }

            if (NewCard != null)
            {
                NewCard.AnchoredPosition = Vector2.zero;
            }

            State = NewState;
        }

        public void SetHandCard(Card NewCard)
        {
            if (NewCard != null)
            {
                HandCard = NewCard;
                NewCard.SetParent(GetTransform());
            }

            Size = HandCard.Size;

            if (BoardPreBattleCard != null)
            {
                BoardPreBattleCard.gameObject.SetActive(false);
            }

            HandCard.gameObject.SetActive(true);
        }

        public void SetBoardPreBattleCard(Card NewCard)
        {
            if (NewCard == null)
                return;

            if (BoardPreBattleCard != null && BoardPreBattleCard != HandCard)
            { 
                BoardPreBattleCard.DestroyUiObject();
            }

            BoardPreBattleCard = NewCard;
            NewCard.SetParent(GetTransform());
            Size = NewCard.Size;

            if (HandCard != null)
            {
                HandCard.gameObject.SetActive(false);
            }

            BoardPreBattleCard.gameObject.SetActive(true);
        }

        public void SetBoardBattleCard(Card NewCard)
        {
            if (NewCard == null)
                return;

            BoardBattleCard = NewCard;
            NewCard.SetParent(GetTransform());

            HandCard.gameObject.SetActive(false);
            BoardPreBattleCard.gameObject.SetActive(false);
            BoardBattleCard.gameObject.SetActive(true);
        }

        public int GetLength()
        {
            if (State == CardState.NoParent || State == CardState.Hand)
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