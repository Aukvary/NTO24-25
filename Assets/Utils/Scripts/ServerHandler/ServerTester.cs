using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO24.Net
{
    public class ServerTester : MonoBehaviour
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private List<Pair<string, string[]>> _resources;

        [SerializeField]
        private List<Pair<string, string[]>> _resourcesAnother;

        [SerializeField]
        private DataSaver _dataSaver;

        private void Start()
        {
            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            yield return ServerHandler.DeleteAllUsers();
        }
    }
}