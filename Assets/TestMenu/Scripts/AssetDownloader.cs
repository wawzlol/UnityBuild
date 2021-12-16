using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class AssetDownloader : MonoBehaviour {

#if UNITY_ANDROID
    private string assetBundleUrl = "https://drive.google.com/uc?export=download&id=1wIdwYq_f5inJ70XHGZSFaKjcRPSBZDxy";
    private string assetBundleManifest = "https://drive.google.com/uc?export=download&id=1SSmwdsM_Ftjo7gnFXkdDJ4fPIu0ibAlr";
    private string load = "https://drive.google.com/uc?export=download&id=1rZ3m9bXuMAVOmf4kwlds1NeBWFr0C1_R";
    private string loadManifest = "https://drive.google.com/uc?export=download&id=1H9CL4ne3pw9kGD_zSuE7uTfE-kDHC-3c";
    private string setting = "https://drive.google.com/uc?export=download&id=12AXV1UCJJAW0SfS4NCkPLT-K33R9mfpv";
    private string settingManifest = "https://drive.google.com/uc?export=download&id=1BOek_0j7YQXu_uRgH5pIRnyEzDhGG5_c";

#else
    private string assetBundleUrl = "https://drive.google.com/uc?export=download&id=1N5Yn5aGv3SVdVOiGkMkI2dG7PyWCY8Xh";
    private string assetBundleManifest = "https://drive.google.com/uc?export=download&id=1OBtNfaLWCkzrSBDdKhSfNaWsTLKAUwWr";
    private string load = "https://drive.google.com/uc?export=download&id=1dTuVFlbAGk4Cgn_VHW68vzQEHJ_f37Gf";
    private string loadManifest = "https://drive.google.com/uc?export=download&id=1_hrGG_vlvw3zlomEgvDi0mEnSx8DV4FF";
    private string setting = "https://drive.google.com/uc?export=download&id=1GclZfRAIXy80AAj3OOz0KaePygzAsn8q";
    private string settingManifest = "https://drive.google.com/uc?export=download&id=1sNoGXmRn8Ny8uY1VLu1Ll4UnKTMu9Q85";
#endif
    private List<AssetFile> assetFiles = new List<AssetFile>();
    private Dictionary<string, bool> DownloadsDone = new Dictionary<string, bool>();

    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image overlay;

    public static AssetDownloader Instance;
    public event Action DownloadDone;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        overlay.gameObject.SetActive(true);
        progressText.gameObject.SetActive(true);
        progressText.text = "Downloading";
        InitializeAssetFile();
    }

    private void InitializeAssetFile()
    {

        string persistentPath = Application.persistentDataPath;
        if (!Directory.Exists($"{persistentPath}/menu"))
        {
            Directory.CreateDirectory($"{persistentPath}/menu");
        }

#if UNITY_ANDROID
        assetFiles.Add(new AssetFile(assetBundleUrl, $"{persistentPath}/Android", "AssetBundle"));
        assetFiles.Add(new AssetFile(assetBundleManifest, $"{persistentPath}/Android.manifest", "AssetBundleManifest"));
        assetFiles.Add(new AssetFile(load, $"{persistentPath}/menu/load", "load"));
        assetFiles.Add(new AssetFile(loadManifest, $"{persistentPath}/menu/load.manifest", "loadManifest"));
        assetFiles.Add(new AssetFile(setting, $"{persistentPath}/menu/setting", "settingManifest"));
        assetFiles.Add(new AssetFile(settingManifest, $"{persistentPath}/menu/setting.manifest", "settingManifest"));
#else
        assetFiles.Add(new AssetFile(assetBundleUrl, $"{persistentPath}/StandaloneWindows", "AssetBundle"));
        assetFiles.Add(new AssetFile(assetBundleManifest, $"{persistentPath}/StandaloneWindows.manifest", "AssetBundleManifest"));
        assetFiles.Add(new AssetFile(load, $"{persistentPath}/menu/load", "load"));
        assetFiles.Add(new AssetFile(loadManifest, $"{persistentPath}/menu/load.manifest", "loadManifest"));
        assetFiles.Add(new AssetFile(setting, $"{persistentPath}/menu/setting", "settingManifest"));
        assetFiles.Add(new AssetFile(settingManifest, $"{persistentPath}/menu/setting.manifest", "settingManifest"));
#endif
        foreach (AssetFile file in assetFiles)
        {
            DownloadsDone[file.Name] = false;
            if (!CheckFileExist(file.Path))
            {
                StartCoroutine(GetRequest(file.Url, file.Path, file.Name));
            }
            else
            {
                DownloadsDone[file.Name] = true;
            }
        }
        CheckDownloadDone();
    }

    private bool CheckFileExist(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        else return false;
    }

    IEnumerator GetRequest(string url, string path, string name)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.Log("Error");
                DownloadsDone[name] = true;
            }
            else
            {
                if (!File.Exists(path))
                {
                    File.WriteAllBytes(path, webRequest.downloadHandler.data);
                }
                DownloadsDone[name] = true;
                CheckDownloadDone();
            }
        }
    }

    private void CheckDownloadDone()
    {
        bool isDone = true;
        foreach (string key in DownloadsDone.Keys)
        {
            if (DownloadsDone[key] == false)
            {
                isDone = false;
                break;
            }
        }
        if (isDone)
        {
            progressText.gameObject.SetActive(false);
            overlay.gameObject.SetActive(false);
            DownloadDone?.Invoke();
        }
    }

    private class AssetFile
    {
        public AssetFile(string url, string path, string name)
        {
            Url = url;
            Path = path;
            Name = name;
        }

        public string Url { get; private set; }
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}