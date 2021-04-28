using UnityEngine;

namespace TimelineHero.Core
{
    public class SceneControllerBase : MonoBehaviour
    {
        private static SceneControllerBase instance = null;
        public static SceneControllerBase Instance
        {
            get
            {
                if (!instance)
                {
                    print("SceneController do not exist on scene");
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