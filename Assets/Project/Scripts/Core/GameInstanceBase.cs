using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimelineHero.Core
{
    public class GameInstanceBase : MonoBehaviour
    {
        private static GameInstanceBase instance = null;

        public static GameInstanceBase Get() => instance;

        private static GameInstance Config;
        private SubsystemCollection Subsystems = new SubsystemCollection();

        private void Awake()
        {
            instance = this;

            Config = GameInstance.Get();
            InitializeSubsystems();

            DontDestroyOnLoad(this);
        }

        protected void InitializeSubsystems()
        {
            Subsystems.Add(new InputSystem());
        }
    }
}