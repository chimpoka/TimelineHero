using UnityEngine;
using TimelineHero.Battle;
using TimelineHero.Core;
using TimelineHero.BattleCardsControl;

namespace TimelineHero.BattleView
{
    public class BattleSystemView : MonoBehaviour
    {
        public HandView PlayerHand;
        public BoardView BattleBoard;
        public DiscardSectionView DiscardSection;

        public BattleController PlayerBattleController;

        public void Initialize()
        {
            BattleSystem.Get().OnInitialBattleState += SetInitialBattleState;
            BattleSystem.Get().OnPlayBattleState += SetPlayBattleState;

            GameInstance.Get().CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            PlayerBattleController = new BattleController();
            PlayerBattleController.Initialize(PlayerHand, BattleBoard, DiscardSection);
        }

        public BattleTimelineTimerView GetTimerView()
        {
            return BattleBoard.TimerView;
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

        public void SetBattleCardsControlStrategy()
        {
            PlayerBattleController.CardsControlStrategy = new BattleCardsControlStrategy(PlayerHand, BattleBoard);
        }

        public void SetDiscardCardsControlStrategy()
        {
            PlayerBattleController.CardsControlStrategy = new DiscardCardsControlStrategy(PlayerHand, BattleBoard, DiscardSection);
        }
    }
}