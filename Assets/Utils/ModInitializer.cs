using UnityEngine;
using UnityEngine.Windows;

namespace NTO24
{
    [GlobalEventListner]
    public class ModInitializer
    {
        private const string PATH = null;

        private static void OnAppStart()
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
                Debug.Log("io");
            }
            else
                Debug.Log("op");
        }
    }
}