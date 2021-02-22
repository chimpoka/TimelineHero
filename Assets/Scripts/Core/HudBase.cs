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
    }
}