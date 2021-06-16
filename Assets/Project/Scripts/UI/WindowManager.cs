using TimelineHero.BattleUI;

namespace TimelineHero.CoreUI
{
    public class WindowManager
    {
        public static System.Action OnDiscardWindowOpened;
        public static System.Action OnDiscardWindowClosed;

        public void OpenWindow<T>(HudBase Hud)
        {
            foreach (var prefab in WindowsContainer.Get().WindowPrefabs)
            {
                if (prefab.GetType() == typeof(T))
                {
                    if (typeof(T) == typeof(DiscardCardWindow))
                    {
                        ShowDiscardCardWindow(Hud, prefab);
                    }
                    else
                    {
                        Hud.InstantiateWindow(prefab);
                    }
                    
                    return;
                }
            }
        }

        private void ShowDiscardCardWindow(HudBase Hud, Window Prefab)
        {
            DiscardCardWindow window = Hud.InstantiateWindow(Prefab) as DiscardCardWindow;
            window.OnWindowClosed = () => OnDiscardWindowClosed?.Invoke();
            OnDiscardWindowOpened?.Invoke();
        }
    }
}