using System.Collections.Generic;

namespace TimelineHero.Core
{
    public class SubsystemCollection
    {
        private Dictionary<System.Type, SubsystemBase> Collection = new Dictionary<System.Type, SubsystemBase>();

        public void Add<T>(T NewSystem) where T : SubsystemBase
        {
            if (Collection.ContainsKey(typeof(T)))
                return;

            Collection.Add(typeof(T), NewSystem);
            NewSystem.Initialize();
        }

        public T GetSubsystem<T>() where T : SubsystemBase
        {
            if (!Collection.ContainsKey(typeof(T)))
                return null;

            return (T)Collection[typeof(T)];
        }
    }
}