using AnantarupaStudios.Input;
using AnantarupaStudios.Menu;
using UnityEngine;

namespace TestMenu.Menu.Load
{
    public class LoadController : MenuController
    {
        #region Fields
        public override string Path => MenuPath.Load.Main;
        public Button back;

        #endregion
        void Start()
        {
            back.ButtonUp += OnBackUp;
        }

        private void OnBackUp()
        {
            MenuManager.Back();
        }
    }
}