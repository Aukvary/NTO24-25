using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

#pragma warning disable CS4014
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
        User.DeleteAll();
        SceneManager.LoadScene((int)Scenes.End);
    }

    public void BadEnd()
    {
        User.DeleteAll();
        SceneManager.LoadScene((int)Scenes.House);
    }

    public void ExitToMenu()
    {
        if (User.Tutorial)
            DeleteTutuorialInfo();
        

        SceneManager.LoadScene((int)Scenes.MainMenu);
    }

    private async void DeleteTutuorialInfo()
    {
        User.Tutorial = false;
        var seed = User.TutorialSeed;
        User.TutorialSeed = null;
        foreach (var user in (await User.GetUsers()).Where(u => u.Name.Contains(seed)))
            user.DeleteUser();
    }
}