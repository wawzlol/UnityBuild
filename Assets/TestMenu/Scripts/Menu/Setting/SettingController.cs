using System;
using AnantarupaStudios.Input;
using AnantarupaStudios.Menu;

namespace TestMenu.Menu.Setting
{
    public class SettingController : MenuController
    {
        #region Fields

        public override string Path => MenuPath.Setting.Main;

        public Button backBtn;
        #endregion
        void Start()
        {
            backBtn.ButtonUp += OnBackButtonUp; 
        }

        private void OnBackButtonUp()
        {
            MenuManager.Back();
        }
    }
}