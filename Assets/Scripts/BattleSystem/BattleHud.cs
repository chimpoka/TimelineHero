using TimelineHero.Hud;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.Battle
{
    public class BattleHud : HudBase
    {
        [SerializeField]
        private Button PlayBattleButton;

        public System.Action OnPlayBattleButtonEvent;

        private BattleSceneController BattleSceneControllerCached;

        public void SetBattleSceneController(BattleSceneController BattleSceneControllerRef)
        {
            BattleSceneControllerCached = BattleSceneControllerRef;
        }

        public void SetPlayState()
        {
            PlayBattleButton.interactable = false;
        }

        public void SetConstructState()
        {
            PlayBattleButton.interactable = true;
        }

        public void OnPlayBattleButton()
        {
            OnPlayBattleButtonEvent();
        }

        public void SlowDownGameSpeed()
        {
            Time.timeScale /= 2;
        }

        public void SpeedUpGameSpeed()
        {
            Time.timeScale *= 2;
        }
    }
}