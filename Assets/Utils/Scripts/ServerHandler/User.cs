using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace NTO24.Net
{
    public class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resources")]
        public Dictionary<string, string[]> Data { get; set; }

        public string[] this[string dataName]
        {
            get => Data[dataName];

            set
            {
                var oldValue = this[dataName];
                Data[dataName] = value;
                PlayerPrefs.SetString(Name, JsonConvert.SerializeObject(Data));

                if (!ServerHandler.HasConnection)
                    return;

                int maxLength = Math.Max(oldValue.Length, value.Length);
                string[] logs = new string[maxLength];
                for (int i = 0; i < maxLength; i++)
                {
                    string oldVal = i < oldValue.Length ? oldValue[i] : "(none)";
                    string newVal = i < value.Length ? value[i] : "(none)";
                    logs[i] = $"  {oldVal} -> {newVal}";
                }

                ServerCoroutineManager
                .Current
                .StartCoroutine(ServerHandler.Log("Local Changes", this, new(dataName, logs)));
            }
        }

        public User(string name, Dictionary<string, string[]> data)
        {
            Name = name;

            Data = data;
        }

        public IEnumerator Update()
        {
            User oldUser = null;
            yield return ServerHandler.GetUser(Name, u => oldUser = u);

            ServerCoroutineManager
                .Current
                .StartCoroutine(ServerHandler.UpdateUser(this));

            foreach (var user in Stuff.ParallelFor(oldUser.Data, Data))
            {
                var old = user.Value1.Value;
                var current = user.Value2.Value;

                int maxLength = Math.Max(old.Length, old.Length);
                string[] logs = new string[maxLength];
                for (int i = 0; i < maxLength; i++)
                {
                    string oldVal = i < old.Length ? old[i] : "(none)";
                    string newVal = i < current.Length ? current[i] : "(none)";
                    logs[i] = $"  {oldVal} -> {newVal}";
                }

                ServerCoroutineManager
                .Current
                .StartCoroutine(ServerHandler.Log("Server Changes", this, new(user.Value1.Key, logs)));
            }

        }

        public string ToJson(bool onlyResources = false)
             => JsonConvert.SerializeObject(onlyResources ? Data : this, Formatting.Indented);
    }
}