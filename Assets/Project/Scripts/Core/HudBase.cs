using UnityEngine;

namespace TimelineHero.CoreUI
{
    public abstract class HudBase : MonoBehaviour
    {
        public UiComponent InstantiateWindow(UiComponent WindowPrefab)
        {
            UiComponent window = Instantiate(WindowPrefab);
            window.SetParent(transform);
            window.SetAnchorsToCenter();

            return window;
        }
    }

    public class Hud<T> : HudBase where T : HudBase
    {
        private static T instance = null;
        public static T Get() => instance;

        private void Awake()
        {
            instance = (T)(HudBase)this;
        }
    }
}