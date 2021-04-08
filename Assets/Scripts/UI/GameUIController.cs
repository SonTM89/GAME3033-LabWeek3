using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameHudWidget GameCanvas;
    [SerializeField] private GameHudWidget PauseCanvas;

    private GameHudWidget ActiveWidget;


    private void Start()
    {
        DisableAllMenus();
        EnableGameMenu();
    }


    public void EnablePauseMenu()
    {
        if (ActiveWidget) ActiveWidget.DisableWidget();

        ActiveWidget = PauseCanvas;

        ActiveWidget.EnableWidget();
    }


    public void EnableGameMenu()
    {
        if (ActiveWidget) ActiveWidget.DisableWidget();

        ActiveWidget = GameCanvas;

        ActiveWidget.EnableWidget();
    }


    public void DisableAllMenus()
    {
        GameCanvas.DisableWidget();
        PauseCanvas.DisableWidget();
    }
}

public abstract class GameHudWidget : MonoBehaviour
{
    public virtual void EnableWidget()
    {
        gameObject.SetActive(true);
    }


    public void DisableWidget()
    {
        gameObject.SetActive(false);
    }
}