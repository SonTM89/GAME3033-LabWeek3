using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Menus
{
    public class MainMenuWidget : MenuWidget
    {
        [SerializeField] private string StartMenuName = "LoadGameMenu";
        [SerializeField] private string OptionsMenuName = "OptionsMenu";


        public void OpenStartMenu()
        {
            
            MenuController.EnableMenu(StartMenuName);
        }


        public void OpenOptionsMenu()
        {

            MenuController.EnableMenu(OptionsMenuName);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}