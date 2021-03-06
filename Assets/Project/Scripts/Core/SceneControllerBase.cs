﻿using UnityEngine;

namespace TimelineHero.Core
{
    public abstract class SceneControllerBase : MonoBehaviour
    {
        protected SubsystemCollection Subsystems = new SubsystemCollection();

        private void Awake()
        {
            if (!GameInstanceBase.Get())
            {
                new GameObject("GameInstance").AddComponent<GameInstanceBase>();
            }

            InitializeSubsystems();
        }

        protected virtual void InitializeSubsystems() {}
    }
}