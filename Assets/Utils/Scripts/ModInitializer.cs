using System.Collections;
using System.IO;
using System.Xml.Linq;
using NTO24.Net;
using NTO24.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Rendering;


namespace NTO24
{
    [GlobalEventListner]
    public class ModInitializer
    {
        private static void OnAppStart()
        {
            ServerCoroutineManager.Current.StartCoroutine(LoadData());
        }

        public static IEnumerator LoadData()
        {
            var path = Path.Combine(Application.persistentDataPath, "Custom Bears");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                var dir = new DirectoryInfo(path);

                foreach (var bear in dir.GetDirectories())
                {
                    var files = bear.GetFiles();
                    string name = bear.Name;
                    string data = null;
                    Texture2D texture = null;
                    Texture2D icon = null;

                    using (UnityWebRequest spriteSRquest = UnityWebRequestTexture
                                .GetTexture("file:///" + bear + "/Sprite.png"))
                    {
                        yield return spriteSRquest.SendWebRequest();

                        texture = DownloadHandlerTexture.GetContent(spriteSRquest);
                    }

                    using (UnityWebRequest iconRequest = UnityWebRequestTexture
                                .GetTexture("file:///" + bear + "/Icon.png"))
                    {
                        yield return iconRequest.SendWebRequest();
                        icon = DownloadHandlerTexture.GetContent(iconRequest);
                    }

                    using StreamReader reader = new(bear + "/Data.txt");

                    data = reader.ReadToEnd();

                    BearInfo.Add(texture, icon, name, data);
                }
            }
            ChangeBearMenu.Initialize();
        }
    }
}