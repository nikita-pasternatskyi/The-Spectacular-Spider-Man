using System.Collections.Generic;
using System.Linq;

namespace MP.Extensions
{
    public static class ListExtensions
    {
        public static bool IsEmptyReliable<T>(this List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }
    }
}