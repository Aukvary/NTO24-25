using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NTO24.Editor
{
    [InitializeOnLoad]
    public static class EditorUpdater
    {
        private static List<IEnumerator> _coroutines = new();

        static EditorUpdater()
        {
            EditorApplication.update += Update;
        }

        public static void AddCoroutine(IEnumerator coroutine)
            => _coroutines.Add(coroutine);

        private static void Update()
        {
            if (_coroutines.Count == 0)
                return;

            for (int i = _coroutines.Count - 1; i >= 0; i--)
                if (!_coroutines[i].MoveNext())
                    _coroutines.RemoveAt(i);
        }
    }
}