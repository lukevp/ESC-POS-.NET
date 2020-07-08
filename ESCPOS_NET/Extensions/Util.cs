using System.Collections.Generic;

namespace ESCPOS_NET.Extensions
{
    public static class Util
    {
        /// <summary>
        /// Adds to the list of T items and/or enumerables of T. All other types will be ignored and not added to the list.
        /// </summary>
        /// <typeparam name="T">List's items type.</typeparam>
        /// <param name="list">List to be added the items.</param>
        /// <param name="items">Items to be added.</param>
        /// <returns>True if no item was ignored, otherwise False.</returns>
        public static bool AddRange<T>(this List<T> list, params object[] items)
        {
            bool ignoredItems = false;
            foreach (var item in items)
            {
                if (item is T itemT)
                {
                    list.Add(itemT);
                }
                else if (item is IEnumerable<T> arrayT)
                {
                    list.AddRange(arrayT);
                }
                else
                {
                    ignoredItems = true;
                }
            }

            return !ignoredItems;
        }
    }
}
