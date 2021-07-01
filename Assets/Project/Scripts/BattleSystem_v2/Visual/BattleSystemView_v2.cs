using System.Linq;
using TimelineHero.Battle_v2;
using TimelineHero.BattleCardsControl;
using TimelineHero.BattleView;
using TimelineHero.Character;
using TimelineHero.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TimelineHero.BattleView_v2
{
    public class BattleSystemView_v2 : MonoBehaviour
    {
        public HandView_v2 PlayerHand;
        public BoardView BattleBoard;

        public BattleController_v2 PlayerBattleController;

        public void Initialize()
        {
            BattleSystem_v2.Get().OnInitialBattleState += SetInitialBattleState;
            BattleSystem_v2.Get().OnPlayBattleState += SetPlayBattleState;

            GameInstance.Get().CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            PlayerHand.Initialize();

            PlayerBattleController = new BattleController_v2();
            PlayerBattleController.Initialize(PlayerHand, BattleBoard);

            PlayerHand.OnCardCreated += SubscribeCardOnPointerEvents;
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