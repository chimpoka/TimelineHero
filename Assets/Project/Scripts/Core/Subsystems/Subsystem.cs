namespace TimelineHero.Core
{
    public abstract class SubsystemBase
    {
        public abstract void Initialize();
    }

    public class Subsystem<T> : SubsystemBase where T : SubsystemBase
    {
        private static T instance = null;
        public static T Get() => instance;

        public override void Initialize()
        {
            instance = (T)(SubsystemBase)this;

            OnInitialize();
        }

        virtual protected void OnInitialize()
        {
        }
    }
}