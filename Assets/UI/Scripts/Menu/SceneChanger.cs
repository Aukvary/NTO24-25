using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private enum Scenes
    {
        MainMenu,
        Tutorial,
        Map,

    }

    [SerializeField]
    private Scenes _scene;

    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeScene()
        => SceneManager.LoadScene((int)_scene);
}