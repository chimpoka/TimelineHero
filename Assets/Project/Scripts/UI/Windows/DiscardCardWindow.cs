using TimelineHero.Battle;
using TimelineHero.BattleView;
using TimelineHero.CoreUI;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineHero.BattleUI
{
    public class DiscardCardWindow : UiComponent
    {
        [SerializeField] private BackgroundWidget Background;
        [SerializeField] private Button OverlookButton;
        [SerializeField] private Button CloseButton;

        public System.Action OnWindowClosed;

        private bool IsEnabled;

        private void Start()
        {
            Show();
        }

        private void Show()
        {
            Background.Show();
            BattleHud.Get().OrderController.SetDiscardCardLayout();
            CloseButton.gameObject.SetActive(true);

            IsEnabled = true;
        }

        private void Hide()
        {
            Background.Hide();
            BattleHud.Get().OrderController.SetOverlookLayout();
            CloseButton.gameObject.SetActive(false);

            IsEnabled = false;
        }

        private void ToggleVisibility()
        {
            if (IsEnabled)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void OnOverlookButton()
        {
            ToggleVisibility();
        }

        public void OnCloseButton()
        {
            DestroyUiObject();
            OnWindowClosed?.Invoke();
        }

        public void OnConfirmButton()
        {
            BattleSystem.Get().DiscardCardsFromDiscardSection();
            DestroyUiObject();
            OnWindowClosed?.Invoke();
        }
    }
}