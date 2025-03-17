using NTO24.Net;
using System.Collections;
using UnityEngine;

namespace NTO24
{
    public class GameEnder : MonoBehaviour
    {
        public void GoToHappyEnd()
            => StartCoroutine(End(Scenes.HappyEnd));

        public void GoToBadEnd()
            => StartCoroutine(End(Scenes.BadEnd));

        public IEnumerator End(Scenes scene)
        {
            yield return ServerHandler.DeleteSave(ServerHandler.ID);
            SceneChanger.Instance.LoadScene((int)scene);
        }
    }
}