using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private enum Scenes
    {
        MainMenu,
        Map,
        Tutorial,
        End,
        House
    }

    public void Exit()
    {
        PlayerPrefs.SetString(nameof(User), User.PlayerID);
        PlayerPrefs.Save();
        PlayerPrefs.DeleteAll();
        User.Tutorial = false;
        User.TutorialSeed = null;
        Application.Quit();
    }

    public void ContinueGame()
    {
        User.Tutorial = false;
        User.PlayerID = PlayerPrefs.GetString(nameof(User), null);
        SceneManager.LoadScene((int)Scenes.Map);
    }

    public void NewGame(TMPro.TMP_InputField field)
    {
        User.Tutorial = false;
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

    public void End()
    {
        User.Delete();
        SceneManager.LoadScene((int)Scenes.End);
    }

    public void BadEnd()
    {
        User.Delete();
        SceneManager.LoadScene((int)Scenes.House);
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