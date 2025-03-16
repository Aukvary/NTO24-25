using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace NTO24.Net
{
    [PreInitialize]
    public static class ServerHandler
    {
        private const string UUID = "99226043-2b2e-420c-a919-ab41b53e6fb2"/*"8114e8ed-83f9-4033-a2eb-8f6c6635fe8d"*/;
        private const string URL = "http://127.0.0.1:8000/";

        private static readonly string _usersURL;
        private static readonly string _logsURL;

        private static List<User> _localUsers;

        public static string ID { get; private set; }

        public static bool HasConnection { get; private set; }

        public static IEnumerable<User> LocalUsers => _localUsers;

        static ServerHandler()
        {
            //_usersURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/";
            //_logsURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/";

            _usersURL = $"http://127.0.0.1:8000/api/games/{UUID}/players/";
            _logsURL = $"http://127.0.0.1:8000/api/games/{UUID}/logs/";
        }

        private static IEnumerator PreInitialize()
        {
            ID = PlayerPrefs.GetString(nameof(User));
            yield return CheckConnection(c => HasConnection = c);
        }

        public static IEnumerator GetServerUsers(UnityAction<List<User>> callBack)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(_usersURL))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    callBack?.Invoke(null);
                    yield break;
                }

                List<User> user = request.downloadHandler.text.ToList<User>();
                callBack?.Invoke(user);
            }
        }

        public static IEnumerator InitializeUser(
            string name,
            Dictionary<string, string[]> baseData,
            UnityAction<User> callBack)
        {
            name = $"{ID}_{name}";
            User loadedEntity = null;

            yield return GetUser(name, e => loadedEntity = e);

            if (loadedEntity != null)
            {
                callBack.Invoke(loadedEntity);
                yield break;
            }

            User newUser = new(name, baseData ?? new Dictionary<string, string[]>());

            using (UnityWebRequest request = new UnityWebRequest(_usersURL, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(newUser.ToJson());

                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callBack.Invoke(newUser);
                    yield break;
                }
            }
            callBack.Invoke(null);
        }

        public static IEnumerator GetUser(string name, UnityAction<User> callBack)
        {
            string url = $"{_usersURL}{name}/";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    callBack?.Invoke(null);
                    yield break;
                }

                User user = request.downloadHandler.text.ToUser();
                callBack?.Invoke(user);
            }
        }

        public static IEnumerator UpdateUser(User user)
        {
            string data = "{\"resources\":" + user.ToJson(true) + "}";

            using (UnityWebRequest request = UnityWebRequest.Put($"{_usersURL}{user.Name}/", data))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                    throw new Exception(request.error);


            }
        }

        public static IEnumerator DeleteUser(User user)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete($"{_usersURL}{user.Name}/"))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                    Debug.LogError(request.error);
                else
                    Debug.Log($"user {user.Name} was Deleted");
            }
        }

        public static IEnumerator DeleteSave(string id)
        {
            var connection = false;

            yield return CheckConnection(c => connection = c);

            if (!connection)
                yield break;

            IEnumerable<User> users = null;
            yield return GetServerUsers(us => users = us.Where(u => u.Name.Split("_")[0] == id));

            if (users != null)
                foreach (var user in users)
                    yield return DeleteUser(user);
            else
                Debug.LogWarning($"server has no users with id {id}");
        }

        public static IEnumerator DeleteAllUsers()
        {
            List<User> users = null;
            yield return GetServerUsers(us => users = us);

            if (users != null)
                foreach (var user in users)
                    yield return DeleteUser(user);
            else
                Debug.LogWarning("server has no users");
        }

        public static IEnumerator Log(string comment, User user, Pair<string, string[]> changes)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append($"\"comment\": \"{comment}\",");
            jsonBuilder.Append($"\"player_name\": \"{user.Name}\",");
            jsonBuilder.Append("\"resources_changed\": {");
            jsonBuilder.Append($"\"{changes.Value1}\": {JsonConvert.SerializeObject(changes.Value2)}");
            jsonBuilder.Append("}}");

            var log = jsonBuilder.ToString();

            using (UnityWebRequest request = new UnityWebRequest(_logsURL, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(log);

                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                    throw new Exception(request.error);
            }
        }

        public static IEnumerator CheckConnection(UnityAction<bool> connect—allback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(_usersURL))
            {
                yield return request.SendWebRequest();
                connect—allback?.Invoke(request.result == UnityWebRequest.Result.Success);
            }
        }
    }
}