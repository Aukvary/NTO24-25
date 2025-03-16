using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

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

                PlayerPrefs.SetString($"{Name}_{dataName}", JsonConvert.SerializeObject(value));

                if (!ServerHandler.HasConnection)
                    return;

                ServerCoroutineManager
                    .Current.StartCoroutine(ServerHandler.UpdateUser(this));

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
                    .StartCoroutine(ServerHandler.Log("Changes", this, new(dataName, logs)));
            }
        }

        public User(string name, Dictionary<string, string[]> data)
        {
            Name = name;

            Data = data;
        }

        public string ToJson(bool onlyResources = false)
             => JsonConvert.SerializeObject(onlyResources ? Data : this, Formatting.Indented);
    }
}