using NTO24.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace NTO24
{
    public class DataSaver : EntityComponent
    {
        [SerializeField]
        private string _name;

        private User _user;

        private Dictionary<string, ISavableComponent> _components;

        protected override void Awake()
        {
            base.Awake();
            _components = GetComponents<ISavableComponent>().ToDictionary(c => c.Name, c => c);

            Main.AddServerRequest(Initialize());
        }

        public IEnumerator Initialize()
        {
            if (ServerHandler.HasConnection && SaveManager.InitializeFrom == InitializeFrom.Server)
                yield return ServerInitialize();
            else
                LocalInitialize();
        }

        private IEnumerator ServerInitialize()
        {
            yield return ServerHandler.InitializeUser(
                _name,
                _components.ToDictionary(p => p.Value.Name, p => p.Value.Data),
                u => _user = u
                );
            foreach (var pair in _user.Data)
            {
                _components[pair.Key].ServerInitialize(_user[pair.Key]);
                _components[pair.Key].OnDataChangeEvent.AddListener(() =>
                {
                    string dataName = pair.Key;
                    _user[pair.Key] = _components[dataName].Data;
                });
            }
        }

        private void LocalInitialize()
        {
            var jsonData = PlayerPrefs.GetString($"{ServerHandler.ID}_{_name}", null);

            Dictionary<string, string[]> initData;

            if (string.IsNullOrEmpty(jsonData))
                initData = _components.ToDictionary(p => p.Value.Name, p => p.Value.Data);
            else
                initData = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonData);

            _user = new($"{ServerHandler.ID}_{_name}", initData);

            foreach (var pair in _user.Data)
            {
                _components[pair.Key].ServerInitialize(_user[pair.Key]);
                _components[pair.Key].OnDataChangeEvent.AddListener(() =>
                {
                    string dataName = pair.Key;
                    _user[pair.Key] = _components[dataName].Data;
                });
            }
        }
    }
}