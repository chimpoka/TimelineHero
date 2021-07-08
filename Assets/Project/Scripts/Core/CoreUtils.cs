using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TimelineHero.Core.Utils
{
    public class CoreUtils
    {
        public static IEnumerable<T> Combine<T>(params ICollection<T>[] toCombine)
        {
            return toCombine.SelectMany(x => x);
        }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (System.Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)System.Activator.CreateInstance(type, constructorArgs));
            }
            
            return objects;
        }
    }
}