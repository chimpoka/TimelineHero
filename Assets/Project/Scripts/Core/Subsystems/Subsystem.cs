using UnityEditor;
using UnityEngine;

namespace TimelineHero.Core
{
    public abstract class CoreSubsystem
    {
        protected abstract void OnInitialize();
        public abstract void Initialize();
    }

    public class Subsystem<T> : CoreSubsystem where T : CoreSubsystem
    {
        private static T instance = null;
        public static T Get() => instance;

        public override void Initialize()
        {
            instance = (T)(CoreSubsystem)this;

            OnInitialize();
        }

        protected override void OnInitialize()
        {
        }
    }
}