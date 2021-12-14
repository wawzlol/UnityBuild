using AnantarupaStudios.Input;
using AnantarupaStudios.Menu;
using UnityEngine;

namespace TestMenu.Menu.Home
{
    public class HomeController : MonoBehaviour, IWidget
    {
        #region Fields

        public Button newGameBtn;
        public Button loadGameBtn;
        public Button settingBtn;

        #endregion
        void Start()
        {
            newGameBtn.ButtonUp += OnNewGameUp;
            loadGameBtn.ButtonUp += OnLoadGameUp;
            settingBtn.ButtonUp += OnSettingUp;
        }

        private void OnNewGameUp()
        {
            MenuManager.Show(MenuPath.NewGame.Main);
        }

        private void OnLoadGameUp()
        {
            MenuManager.Show(MenuPath.Load.Main);
        }

        private void OnSettingUp()
        {
            MenuManager.Show(MenuPath.Setting.Main);
        }

        public void OnMenuChanged(string path)
        {
            gameObject.SetActive(path == MenuPath.Home.Main);
        }
    }
}