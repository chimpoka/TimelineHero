using TimelineHero.Character;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TimelineHero.BattleView
{
    public enum CardState { Hand, BoardPrePlay, BoardPlay, Discard }

    public class CardWrapper : UiComponent, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public int Length { get => GetLength(); }

        [HideInInspector] public EquipmentDeckView EquipmentDeckCached;

        [HideInInspector] public Card HandCard;
        [HideInInspector] public Card BoardPreBattleCard;
        [HideInInspector] public Card BoardBattleCard;
        [HideInInspector] public CardState State;

        public delegate void CardEventHandler(CardWrapper card, PointerEventData eventData);
        public event CardEventHandler OnPointerDownEvent;
        public event CardEventHandler OnPointerUpEvent;
        public event CardEventHandler OnBeginDragEvent;
        public event CardEventHandler OnEndDragEvent;
        public event CardEventHandler OnDragEvent;

        private void Awake()
        {
            HandCard = CreateCard();
            BoardPreBattleCard = CreateCard();
            BoardBattleCard = CreateCard();
        }

        private Card CreateCard()
        {
            Card card = MonoBehaviour.Instantiate(BattlePrefabsConfig.Get().CardPrefab);
            card.SetParent(GetTransform());
            card.AnchoredPosition = Vector2.zero;

            return card;
        }

        public void SetState(CardState NewState, Skill NewSkill)
        {
            if (NewState == CardState.Hand)
            {
                HandCard.SetSkill(NewSkill);
                Size = HandCard.Size;
                HandCard.GetTransform().SetSiblingIndex(2);
            }
            else if (NewState == CardState.BoardPrePlay)
            {
                BoardPreBattleCard.SetSkill(NewSkill);
                Size = BoardPreBattleCard.Size;
                BoardPreBattleCard.GetTransform().SetSiblingIndex(2);
                BoardPreBattleCard.PlayRestoreAnimation();
            }
            else if (NewState == CardState.BoardPlay)
            {
                BoardBattleCard.SetSkill(NewSkill);
                Size = BoardBattleCard.Size;
                BoardBattleCard.GetTransform().SetSiblingIndex(1);

                SkillUtils.CopyCardStats(BoardBattleCard, BoardPreBattleCard);
                BoardPreBattleCard.PlayDestroyAnimation();
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

        public Skill GetSkill()
        {
            if (State == CardState.Hand)
            {
                return HandCard.GetSkill();
            }
            if (State == CardState.BoardPrePlay)
            {
                return BoardPreBattleCard.GetSkill();
            }
            if (State == CardState.BoardPlay)
            {
                return BoardBattleCard.GetSkill();
            }
            return null;
        }

        public Skill GetOriginalSkill()
        {
            return HandCard.GetSkill();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(new Vector3(WorldPosition.x, WorldPosition.y, 2000), new Vector3(0.1f, 0.1f, 0.2f));
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