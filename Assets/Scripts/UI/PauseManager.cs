using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;



    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        UnpauseGame();
    }


    public void PauseGame()
    {
        var pausables = FindObjectsOfType<MonoBehaviour>().OfType<IPausable>();

        foreach(IPausable pausable in pausables)
        {
            pausable.PauseMenu();
        }

        Time.timeScale = 0;

        AppEvents.Invoke_OnMouseCursorEnable(true);
    }


    public void UnpauseGame()
    {
        var pausables = FindObjectsOfType<MonoBehaviour>().OfType<IPausable>();

        foreach (IPausable pause in pausables)
        {
            pause.UnPauseMenu();
        }

        Time.timeScale = 1;
        AppEvents.Invoke_OnMouseCursorEnable(false);
    }

    private void OnDestroy()
    {
        //UnpauseGame();
    }
}


interface IPausable
{
    void PauseMenu();
    void UnPauseMenu();
}
