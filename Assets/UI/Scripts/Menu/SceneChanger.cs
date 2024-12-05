using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class SceneChanger : MonoBehaviour
{
    private enum Scenes
    {
        MainMenu,
        Map,
        Tutorial,

    }

    public async void Exit()
    {
        PlayerPrefs.SetString(nameof(User), User.PlayerID);
        PlayerPrefs.Save();
        if (User.Tutorial)
        {
            foreach (var user in (await User.GetUsers()).Where(u => u.Name.Contains(User.TutorialSeed)))
                user.DeleteUser();

        }
        User.Tutorial = false;
        User.TutorialSeed = null;
        Application.Quit();
    }

    public void ContinueGame()
    {

        SceneManager.LoadScene((int)Scenes.Map);
    }

    public void NewGame(TMPro.TMP_InputField field)
    {
        PlayerPrefs.SetString(nameof(User), field.text);
        PlayerPrefs.Save();

        User.PlayerID = field.text;

        SceneManager.LoadScene((int)Scenes.Map);
    }

    public void StartTutorial()
    {
        User.TutorialSeed = DateTime.Now.Ticks.ToString();
        User.Tutorial = true;
        SceneManager.LoadScene((int)Scenes.Tutorial);
    }

    public async void ExitToMenu()
    {
        if (User.Tutorial)
        {
            foreach (var user in (await User.GetUsers()).Where(u => u.Name.Contains(User.TutorialSeed)))
                user.DeleteUser();

        }
        User.Tutorial = false;
        User.TutorialSeed = null;

        SceneManager.LoadScene((int)Scenes.MainMenu);
    }
}