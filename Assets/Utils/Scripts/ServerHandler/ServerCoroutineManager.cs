using UnityEngine;

namespace NTO24.Net
{
    public class ServerCoroutineManager : MonoBehaviour
    {
        public static ServerCoroutineManager Current { get; private set; }

        private void Awake()
        {
            Current = this;
        }
    }
}