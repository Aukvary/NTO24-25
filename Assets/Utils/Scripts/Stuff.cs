using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24
{
    public static class Stuff
    {
        public const string MOUSEX = "Mouse X";
        public const string MOUSEY = "Mouse Y";
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";

        public static IEnumerable<Pair<T1, T2>> ParallelFor<T1, T2>(IEnumerable<T1> first, IEnumerable<T2> second)
        {
            var minLenght = Mathf.Min(first.Count(), second.Count());

            for (int i = 0; i < minLenght; i++)
                yield return new(first.ElementAt(i), second.ElementAt(i));
        }
    }
}