using System.Collections.Generic;
using UnityEngine;

namespace Helpers.Utility
{
    public static class Utils
    {
        public static T RandomElement<T>(this IList<T> list)
        {
            if (list.Count <= 0) return default;
            return list[Random.Range(0, list.Count)];
        }
        
        public static T RandomElementRemove<T>(this IList<T> list)
        {
            if (list.Count <= 0) return default;
            var randomIndex = Random.Range(0, list.Count);
            var retVal = list[randomIndex];
            list.RemoveAt(randomIndex);
            return retVal;
        }
    }
}