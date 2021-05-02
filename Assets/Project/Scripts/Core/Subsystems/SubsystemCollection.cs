using System.Collections.Generic;

namespace TimelineHero.Core
{
    public class SubsystemCollection
    {
        private Dictionary<System.Type, CoreSubsystem> Collection = new Dictionary<System.Type, CoreSubsystem>();

        public void Add<T>(T NewSystem) where T : CoreSubsystem
        {
            if (Collection.ContainsKey(typeof(T)))
                return;

            Collection.Add(typeof(T), NewSystem);
            NewSystem.Initialize();
        }

        public T GetSubsystem<T>() where T : CoreSubsystem
        {
            if (!Collection.ContainsKey(typeof(T)))
                return null;

            return (T)Collection[typeof(T)];
        }
    }
}