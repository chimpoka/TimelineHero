using System.Linq;
using UnityEngine;

namespace TimelineHero.Core
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                _instance = _instance ?? Resources.LoadAll<T>("").FirstOrDefault();
                return _instance;
            }
        }
    }
}