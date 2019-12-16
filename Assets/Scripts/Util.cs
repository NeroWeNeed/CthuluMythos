using System.Collections.Generic;

namespace CMythos
{
    public static class Util
    {
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
        public static bool ContainsAll<T>(this IList<T> ts, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!ts.Contains(item))
                    return false;
            }
            return true;
        }
    }

}