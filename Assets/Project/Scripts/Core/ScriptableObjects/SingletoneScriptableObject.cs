using System.Linq;
using UnityEngine;

namespace TimelineHero.Core
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance = null;
        public static T Get()
        {
            instance = instance ?? Resources.LoadAll<T>("").FirstOrDefault();
            return instance;
        }
    }
}