using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.CoreUI
{
    public class BackgroundWidget : UiComponent
    {
        private Animator AnimController;

        private void Awake()
        {
            AnimController = GetComponent<Animator>();

            if (AnimController == null)
                Debug.LogError("BackgroundWidget::AnimController == null");
        }

        private void Start()
        {
            StretchAnchors();
        }

        public void Show()
        {
            AnimController.SetBool("Show", true);
        }

        public void Hide()
        {
            AnimController.SetBool("Show", false);
        }

        public void ToggleVisibility()
        {
            AnimController.SetBool("Show", !GetState());
        }

        private bool GetState()
        {
            return AnimController.GetBool("Show");
        }
    }
}