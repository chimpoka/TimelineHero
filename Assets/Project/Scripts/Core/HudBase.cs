using TimelineHero.CoreUI;
using UnityEngine;

namespace TimelineHero.Hud
{
    public class HudBase : MonoBehaviour
    {
        private static HudBase instance = null;
        public static HudBase Instance
        {
            get
            {
                if (!instance)
                {
                    print("Hud do not exist on scene");
                    return null;
                }
                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public void InstantiateWindow(UiComponent WindowPrefab)
        {
            UiComponent window = Instantiate(WindowPrefab);
            window.SetParent(transform);
            window.SetAnchorsToCenter();
        }
    }
}