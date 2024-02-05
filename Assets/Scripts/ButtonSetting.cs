using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSetting : MonoBehaviour
{
    public void ButtonClick(string _setting)
    {
        if(_setting == "easy")
        {
            Settings.difficulty = Settings.Difficulties.EASY;
        }
        else if (_setting == "medium")
        {
            Settings.difficulty = Settings.Difficulties.MEDIUM;
        }
        else if (_setting == "hard")
        {
            Settings.difficulty = Settings.Difficulties.HARD;
        }
        else if (_setting == "insane")
        {
            Settings.difficulty = Settings.Difficulties.INSANE;
        }
        else
        {
            Settings.difficulty = Settings.Difficulties.DEBUG;
        }

        SceneManager.LoadScene("GameScene");
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

}
