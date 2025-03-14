using UnityEditor;
using System.Collections;
using NTO24.Net;

namespace NTO24.Editor
{
    public static class ServerEditor
    {
        [MenuItem("Server/DeleteAllUsers")]
        public static void MenuDeleteAllUsers()
        {
            var coroutine = ServerHandler.DeleteAllUsers();

            EditorUpdater.AddCoroutine(coroutine);
        }
    }
}