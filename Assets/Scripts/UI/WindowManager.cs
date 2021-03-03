using TimelineHero.Battle;
using TimelineHero.Core;
using TimelineHero.Hud;
using UnityEngine;

namespace TimelineHero.UI
{
    [CreateAssetMenu(menuName = "ScriptableObject/WindowManager")]
    public class WindowManager : SingletonScriptableObject<WindowManager>
    {
        public LoseWindow LoseWindowPrefab;
        public WinWindow WinWindowPrefab;

        public void ShowLoseWindow()
        {
            HudBase.Instance.InstantiateWindow(LoseWindowPrefab);
        }

        public void ShowWinWindow()
        {
            HudBase.Instance.InstantiateWindow(WinWindowPrefab);
        }
    }
}