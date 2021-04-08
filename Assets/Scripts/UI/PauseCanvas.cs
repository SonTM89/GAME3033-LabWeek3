using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : GameHudWidget
{
    public void ResumeGame()
    {
        PauseManager.Instance.UnpauseGame();
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }


    public void QuitApplication()
    {
        Application.Quit();
    }
}
