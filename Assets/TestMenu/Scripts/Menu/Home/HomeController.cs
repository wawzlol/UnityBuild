using AnantarupaStudios.Input;
using AnantarupaStudios.Menu;
using System;
using UnityEngine;

namespace TestMenu.Menu.Home
{
    public class HomeController : MonoBehaviour, IWidget
    {
        #region Fields

        private bool canPressed = false;
        public Button loadGameBtn;
        public Button settingBtn;

        #endregion
        private void Awake()
        {
            AssetDownloader.Instance.DownloadDone += OnDownloadDone;
        }

        void Start()
        {
            loadGameBtn.ButtonUp += OnLoadGameUp;
            settingBtn.ButtonUp += OnSettingUp;
            
        }

        private void OnDownloadDone()
        {
            canPressed = true;
        }

        private void OnLoadGameUp()
        {
            if (canPressed)
            {
                MenuManager.Show(MenuPath.Load.Main);
            }
        }

        private void OnSettingUp()
        {
            if (canPressed)
            {
                MenuManager.Show(MenuPath.Setting.Main);
            }
        }

        public void OnMenuChanged(string path)
        {
            gameObject.SetActive(path == MenuPath.Home.Main);
        }
    }
}