using System.Collections.Generic;
using System.Linq;

namespace TimelineHero.Core.Utils
{
    public class CoreUtils
    {
        public static IEnumerable<T> Combine<T>(params ICollection<T>[] toCombine)
        {
            return toCombine.SelectMany(x => x);
        }
    }
}