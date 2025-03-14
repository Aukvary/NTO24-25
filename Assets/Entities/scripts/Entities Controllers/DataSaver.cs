using UnityEngine;
using NTO24.Net;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace NTO24
{
    public class DataSaver : EntityComponent
    {
        [SerializeField]
        private string _name;

        private User _user;

        private ISavableComponent[] _components;
        private Dictionary<string, ISavableComponent> _stringPairs;

        protected override void Awake()
        {
            base.Awake();
            _components = GetComponents<ISavableComponent>();
            _stringPairs = _components.ToDictionary(c => c.Name, c => c);

            Main.AddServerRequest(Initialize());
        }

        public IEnumerator Initialize()
        {
            yield return ServerHandler.InitializeUser(
                _name,
                _components.ToDictionary(c => c.Name, c => c.Data),
                u => _user = u
                );

            foreach (var pair in _user.Resources)
            {
                _stringPairs[pair.Key].ServerInitialize(_user[pair.Key]);
                _stringPairs[pair.Key].OnDataChangeEvent.AddListener(() =>
                {
                    string dataName = pair.Key;
                    _user[pair.Key] = _stringPairs[dataName].Data;
                });
            }
        }
    }
}