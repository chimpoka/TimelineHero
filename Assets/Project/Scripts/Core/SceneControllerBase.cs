using UnityEngine;

namespace TimelineHero.Core
{
    public abstract class SceneControllerBase : MonoBehaviour
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

        protected SubsystemCollection Subsystems = new SubsystemCollection();

        private void Awake()
        {
            instance = this;

            GameInstanceBase gameInstance = new GameObject("GameInstance").AddComponent<GameInstanceBase>();

            InitializeSubsystems();
        }

        protected abstract void InitializeSubsystems();
    }
}