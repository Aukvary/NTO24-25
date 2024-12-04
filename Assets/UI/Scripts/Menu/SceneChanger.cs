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

        User.PlayerID = null;
        Application.Quit();
    }

    public void ChangeScene()
        => SceneManager.LoadScene((int)_scene);

    public void NewGame(TMPro.TMP_InputField field)
    {
        PlayerPrefs.SetString(nameof(User.PlayerID), field.text);
        PlayerPrefs.Save();

        User.PlayerID = field.text;

        SceneManager.LoadScene((int)Scenes.Map);
    }
}