using System.Text.Json.Serialization;
using UnityEngine;

namespace NTO24
{
    [System.Serializable]
    public struct Pair<T1, T2>
    {
        [field: SerializeField]
        [JsonPropertyName("value1")]
        public T1 Value1 { get; private set; }

        [field: SerializeField]
        [JsonPropertyName("value2")]
        public T2 Value2 { get; private set; }

        public Pair(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public static Pair<Resource, int> FromJson(string json)
        {
            return default;
        }

        public override string ToString()
        {
            if (typeof(T1) == typeof(Resource) && typeof(T2) == typeof(int))
                return $"{(Value1 as Resource).ResourceName}:{Value2}";

            return $"{Value1}:{Value2}";
        }
    }
}