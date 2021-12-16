using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnantarupaStudios.Utility
{
	public static partial class ResourceManager
	{
		public enum AssetSource
		{
			AssetBundle,
			Resource
		}

		private static Dictionary<string, AssetBundle> LoadedAssetBundle { get; } = new Dictionary<string, AssetBundle>();
		private static AssetBundleManifest AssetBundleManifest { get; set; }

#if UNITY_EDITOR
		private const string editorPrefPath = "Anantarupa.ResourceManager";
		private const string simulateAssetBundleMenu = "ResourceManager/Simulate AssetBundle";
		[MenuItem(simulateAssetBundleMenu)]
		private static void ToggleSimulateAssetBundle()
		{
			EditorPrefs.SetBool(editorPrefPath, !EditorPrefs.GetBool(editorPrefPath));
			UnityEditor.Menu.SetChecked(simulateAssetBundleMenu, EditorPrefs.GetBool(editorPrefPath));
		}

		[InitializeOnLoadMethod]
		public static void Initialize()
		{
			UnityEditor.Menu.SetChecked(simulateAssetBundleMenu, EditorPrefs.GetBool(editorPrefPath));
		}
#endif

		public static T Load<T>(AssetSource source, string path, string name) where T : Object
		{
			switch (source)
			{
				case AssetSource.AssetBundle:
#if UNITY_EDITOR
					if (EditorPrefs.GetBool(editorPrefPath))
					{
						if (AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(path, name).Length < 1)
						{
							Debug.LogError($"Failed to load asset : {name}  | path : {path}");
						}
						return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(path, name)[0]);
					}
#endif

					if (AssetBundleManifest is null)
					{

							AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" +
#if UNITY_ANDROID
							"Android"
#elif UNITY_STANDALONE_WIN
							"StandaloneWindows"
#endif
							);
							AssetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");

					}
					if (!LoadedAssetBundle.ContainsKey(path))
					{
						foreach (string assetBundle in AssetBundleManifest.GetAllDependencies(path))
						{
							if (!LoadedAssetBundle.ContainsKey(assetBundle))
							{
								LoadedAssetBundle[assetBundle] = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + assetBundle);
							}
						}
						LoadedAssetBundle[path] = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + path);
					}
					return LoadedAssetBundle[path].LoadAsset<T>(name);
				case AssetSource.Resource:
					return Resources.Load<T>(path + "/" + name);
			}
			return null;
		}

		public static AsyncOperation LoadSceneAdditive(AssetSource assetSource, string path)
		{
#if UNITY_EDITOR
			if (EditorPrefs.GetBool(editorPrefPath))
			{
				return EditorSceneManager.LoadSceneAsyncInPlayMode(AssetDatabase.GetAssetPathsFromAssetBundle(path)[0], new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive });
			}
#endif
			if (AssetBundleManifest is null)
			{

					AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" +
#if UNITY_ANDROID
							"Android"
#elif UNITY_STANDALONE_WIN
							"StandaloneWindows"
#endif
							);
					AssetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
				
			}
			if (!LoadedAssetBundle.ContainsKey(path))
			{
				foreach (string assetBundle in AssetBundleManifest.GetAllDependencies(path))
				{
					if (!LoadedAssetBundle.ContainsKey(assetBundle))
					{
						LoadedAssetBundle[assetBundle] = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + assetBundle);
					}
				}
				LoadedAssetBundle[path] = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + path);
			}
			return SceneManager.LoadSceneAsync(LoadedAssetBundle[path].GetAllScenePaths()[0], LoadSceneMode.Additive);
		}
	}
}