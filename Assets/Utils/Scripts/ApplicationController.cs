using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NTO24
{
    public class ApplicationController : MonoBehaviour
    {
        private void Start()
        {
            var types = Assembly.GetAssembly(typeof(GlobalEventListnerAttribute)).GetTypes()
                .Where(t => t.GetCustomAttribute<GlobalEventListnerAttribute>() != null);

            foreach (var type in types)
            {
                try
                {
                    type.GetMethod("OnAppStart", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        .Invoke(null, null);
                }
                catch (Exception ex){
                    print(ex.TargetSite.Name);
                    print($"{type.Name} has no static method OnAppStart"); }
            }
        }
    }
}