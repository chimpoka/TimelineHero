using TimelineHero.BattleUI;
using TimelineHero.BattleView;
using TimelineHero.Core;
using UnityEngine;

namespace TimelineHero.CoreUI
{
    [CreateAssetMenu(menuName = "ScriptableObject/WindowManager")]
    public class WindowManager : SingletonScriptableObject<WindowManager>
    {
        public LoseWindow LoseWindowPrefab;
        public WinWindow WinWindowPrefab;
        public DiscardCardWindow DiscardCardWindowPrefab;

        public System.Action OnDiscardWindowOpened;
        public System.Action OnDiscardWindowClosed;

        public void ShowLoseWindow()
        {
            BattleHud.Get().InstantiateWindow(LoseWindowPrefab);
        }

        public void ShowWinWindow()
        {
            BattleHud.Get().InstantiateWindow(WinWindowPrefab);
        }

        public void ShowDiscardCardWindow()
        {
            DiscardCardWindow window = BattleHud.Get().InstantiateWindow(DiscardCardWindowPrefab) as DiscardCardWindow;
            window.OnWindowClosed = () => OnDiscardWindowClosed?.Invoke();

            OnDiscardWindowOpened?.Invoke();
        }
    }
}