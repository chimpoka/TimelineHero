using TimelineHero.Battle;
using TimelineHero.BattleCardsControl;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.BattleView
{
    public class BattleSystemView : MonoBehaviour
    {
        public HandView PlayerHand;
        public BoardView BattleBoard;

        public BattleController PlayerBattleController;

        public void Initialize()
        {
            BattleSystem.Get().OnInitialBattleState += SetInitialBattleState;
            BattleSystem.Get().OnPlayBattleState += SetPlayBattleState;

            GameInstance.Get().CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            PlayerBattleController = new BattleController();
            PlayerHand.OnCardCreated += SubscribeCardOnPointerEvents;
            PlayerHand.Initialize();
            PlayerBattleController.Initialize(PlayerHand, BattleBoard);
        }

        public void SetActive(bool Active)
        {
            PlayerBattleController.SetActive(Active);
        }

        public void SetInitialBattleState()
        {
            SetActive(true);
        }

        public void SetPlayBattleState()
        {
            SetActive(false);
            BattleBoard.SetPlayBattleState();
        }

        private void SubscribeCardOnPointerEvents(CardWrapper PlayerCard)
        {
            PlayerCard.OnPointerDownEvent += PlayerBattleController.OnCardPointerDown;
            PlayerCard.OnPointerUpEvent += PlayerBattleController.OnCardPointerUp;
            PlayerCard.OnDragEvent += PlayerBattleController.OnCardDrag;
        }
    }
}