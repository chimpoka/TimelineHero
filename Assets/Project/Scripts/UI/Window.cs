using System;
using TimelineHero.Core;
using TimelineHero.MapView;
using UnityEngine;

namespace TimelineHero.CoreUI
{
    public class Window : UiComponent
    {
        public bool AddBackground = true;
        
        public System.Action OnStartOpenWindow;
        public System.Action OnOpenWindow;
        public System.Action OnStartCloseWindow;
        public System.Action OnCloseWindow;
        
        protected BackgroundWidget Background;
        
        private void Start()
        {
            InputSystem.Get().bWorldInputEnabled = false;
            
            if (AddBackground)
            {
                Background = Instantiate(CorePrefabsConfig.Get().BackgroundWidgetPrefab);

                if (Background)
                {
                    Background.SetParent(transform);
                    Background.transform.SetSiblingIndex(0);
                    Background.Show();
                }
            }

            StartOpenWindowEvent();
            OnStartOpenWindow?.Invoke();
        }

        public void CloseThisWindow()
        {
            CloseWindowEvent();
            DestroyUiObject();
            OnCloseWindow?.Invoke();
        }

        private void OnDestroy()
        {
            // TODO: Add counter for opened windows
            InputSystem.Get().bWorldInputEnabled = true;
        }

        protected virtual void StartOpenWindowEvent() {}
        protected virtual void OpenWindowEvent() {}
        protected virtual void StartCloseWindowEvent() {}
        protected virtual void CloseWindowEvent() {}
    }
}