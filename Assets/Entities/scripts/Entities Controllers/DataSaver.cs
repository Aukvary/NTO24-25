using Newtonsoft.Json;
using NTO24.Net;
using NTO24.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            /*foreach (var pair in _components)
                print(pair.Key);*/
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

            GameMenu.AddOnExitAction(_user.Update);
            foreach (var pair in _user.Data)
            {
                _components[pair.Key].ServerInitialize(_user[pair.Key]);
                GameMenu.AddOnExitAction(() => LocalUpdate(pair.Key));
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

                GameMenu.AddOnExitAction(() => LocalUpdate(pair.Key));

                _components[pair.Key].OnDataChangeEvent.AddListener(() =>
                {
                    string dataName = pair.Key;
                    _user[pair.Key] = _components[dataName].Data;
                });
            }
        }

        private IEnumerator LocalUpdate(string key)
        {
                _components[key].OnDataChangeEvent.Invoke();
                return null;
        }

        private IEnumerator UpdateLocalInfo()
        {
            yield return new WaitForSeconds(60);
            foreach (var component in _components.Values)
                component.OnDataChangeEvent.Invoke();
        }

        private IEnumerator UpdateServerInfo()
        {
            yield return new WaitForSeconds(600);

            _user.Update();
        }
    }
}