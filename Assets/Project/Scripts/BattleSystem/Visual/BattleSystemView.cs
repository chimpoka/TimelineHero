using UnityEngine;
using TimelineHero.Battle;
using TimelineHero.Core;

namespace TimelineHero.BattleView
{
    public class BattleSystemView : MonoBehaviour
    {
        public HandView PlayerHand;
        public BoardView BattleBoard;

        public BattleController PlayerBattleController;

        public void Initialize()
        {
            BattleSystem.Get().OnConstructState += InitializeConstructState;
            BattleSystem.Get().OnPlayState += InitializePlayState;

            GameInstance.Instance.CanvasScaleFactor = GetComponent<Canvas>().scaleFactor;

            PlayerBattleController = new BattleController();
            PlayerBattleController.Initialize(PlayerHand, BattleBoard);
        }

        public BattleTimelineTimerView GetTimerView()
        {
            return BattleBoard.TimerView;
        }

        public void SetActive(bool Active)
        {
            PlayerBattleController.SetActive(Active);
        }

        public void InitializeConstructState()
        {
            SetActive(true);
            BattleBoard.OnStartConstructState();
        }

        public void InitializePlayState()
        {
            SetActive(false);
            BattleBoard.OnStartPlayState();
        }
    }
}