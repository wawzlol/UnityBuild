using AnantarupaStudios.Utility;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AnantarupaStudios.Menu
{
	public class MenuManager : MonoBehaviour
	{
		public Camera UICamera => uiCamera;

		[SerializeField] protected Camera uiCamera;

		[ShowInInspector, ReadOnly] private readonly Dictionary<string, IMenuController> Menus = new Dictionary<string, IMenuController>();

		[ShowInInspector, ReadOnly] private readonly List<IWidget> Widgets = new List<IWidget>();

		[ShowInInspector, ReadOnly] private readonly Stack<string> menuStack = new Stack<string>();

		[ShowInInspector, ReadOnly] private readonly PopupStack popupStack = new PopupStack();

		[ShowInInspector, ReadOnly] private bool canBackEscape = true;

		public static MenuManager Instance { get; private set; }

		public static IMenuController Show(string path, params object[] data)
		{
			return Instance.InternalShow(path, data);
		}

		private IMenuController InternalShow(string path, params object[] data)
		{
			string menu = path.Split('/')[0];
			Debug.Log(menu);
			if (string.IsNullOrEmpty(menu))
			{
				
				Debug.LogError($"invalid path {path}");
			}
			if (!Menus.TryGetValue(menu, out IMenuController menuController))
			{
				GameObject menuObject = Instantiate(ResourceManager.Load<GameObject>(ResourceManager.AssetSource.AssetBundle, $"menu/{menu}", $"{menu}Canvas"));
				
				Menus[menu] = menuController = menuObject.GetComponent<IMenuController>();
				menuObject.GetComponent<Canvas>().worldCamera = uiCamera;
			}
			menuController.Show(path, data);
			return menuController;
		}

		public static Task<T> ShowPopup<T>(string path, params object[] data)
		{
			return Instance.InternalShowPopup<T>(path, data);
		}

		private Task<T> InternalShowPopup<T>(string path, params object[] data)
		{
			if (menuStack.Count > 0)
			{
				string currentPath = menuStack.Peek();
				Menus[currentPath.Split('/')[0]].Disable(currentPath);
			}

			string popup = path.Split('/')[0];
			GameObject popupObject = Instantiate(ResourceManager.Load<GameObject>(ResourceManager.AssetSource.AssetBundle, $"menu/{popup}", $"{popup}Popup"), transform);
			popupObject.transform.parent = null;
			IPopupController<T> popupController = popupObject.GetComponent<IPopupController<T>>();
			popupObject.GetComponent<Canvas>().worldCamera = uiCamera;
			Task<T> task = popupController.Show(data);
			popupStack.Push(popupController);
			InternalUpdateWidget();
			return task;
		}

		public static void PushMenu(string path, bool hidePrevious = true)
		{
			Instance.InternalPushMenu(path, hidePrevious);
		}

		private void InternalPushMenu(string path, bool hidePrevious = true)
		{
			if (menuStack.Count > 0)
			{
				string currentPath = menuStack.Peek();
				if (currentPath == path) return;
				if (!path.StartsWith(currentPath + '/'))
				{
					if (hidePrevious)
					{
						Menus[menuStack.Peek().Split('/')[0]].Hide(menuStack.Peek());

						Stack<string> temp = new Stack<string>();
						while (!Menus[menuStack.Peek().Split('/')[0]].HidePreviousMenu)
						{
							temp.Push(menuStack.Pop());
							Menus[menuStack.Peek().Split('/')[0]].Hide(menuStack.Peek());
						}

						while (temp.Count > 0)
						{
							menuStack.Push(temp.Pop());
						}
					}
					else
					{
						Menus[currentPath.Split('/')[0]].Disable(currentPath);
					}
				}
			}
			menuStack.Push(path);
			InternalUpdateWidget();
		}

		public static bool Pop(string path, bool updateWidget = true)
		{
			return Instance.InternalPop(path, updateWidget);
		}

		private bool InternalPop(string path, bool updateWidget)
		{
			if (popupStack.Count > 0)
			{
				if (!string.IsNullOrEmpty(path))
					if (popupStack.Peek().Path != path && popupStack.Peek().Path.StartsWith(path + '/'))
						return false;
				IPopupController popupController = popupStack.Pop();
				popupController.Close();
				NotifyPopupListeners(popupController.Path);
				if (menuStack.Count > 0)
				{
					Menus[menuStack.Peek().Split('/')[0]].Enable(menuStack.Peek());
				}
				if (updateWidget) InternalUpdateWidget();
				return true;
			}
			else
			{
				if (menuStack.Count == 0) return false;

				if (!string.IsNullOrEmpty(path))
					if (menuStack.Peek() != path && !menuStack.Peek().StartsWith(path + '/'))
						return false;

				string currentPath = menuStack.Pop();
				Menus[currentPath.Split('/')[0]].Close(currentPath);
				NotifyMenuListeners(currentPath);
				if (menuStack.Count == 0)
				{
					if (updateWidget) InternalUpdateWidget();
					return true;
				}

				currentPath = menuStack.Peek();

				Menus[currentPath.Split('/')[0]].Unhide(currentPath);

				Stack<string> temp = new Stack<string>();
				while (!Menus[menuStack.Peek().Split('/')[0]].HidePreviousMenu)
				{
					temp.Push(menuStack.Pop());
					Menus[menuStack.Peek().Split('/')[0]].Unhide(menuStack.Peek());
				}

				while (temp.Count > 0)
				{
					menuStack.Push(temp.Pop());
				}

				if (updateWidget) InternalUpdateWidget();
				return true;
			}
		}

		private void NotifyMenuListeners(string path)
		{
			foreach (IWidget widget in Widgets)
			{
				if (widget is IMenuListener menuListener)
				{
					menuListener.OnMenuClosed(path);
				}
			}
		}

		private void NotifyPopupListeners(string path)
		{
			foreach (IWidget widget in Widgets)
			{
				if (widget is IPopupListener popupListener)
				{
					popupListener.OnPopupClosed(path);
				}
			}
		}

		public static void UpdateWidget()
		{
			Instance.InternalUpdateWidget();
		}

		private void InternalUpdateWidget()
		{
			string path = "";

			if (menuStack.Count > 0)
			{
				path = menuStack.Peek();
			}

			foreach (IWidget widget in Widgets)
			{
				widget.OnMenuChanged(path);
			}
		}

		private void Awake()
		{
			Instance = this;

			foreach (GameObject gameObject in FindObjectsOfType<GameObject>())
			{
				foreach (IWidget widget in gameObject.GetComponents<IWidget>())
				{
					Widgets.Add(widget);
				}
			}

			OnAwake();
		}

		#region New Function for Tutorial
		public static void EnableBackEscape()
		{
			Instance.InternalEnableBackEscape();
		}

		private void InternalEnableBackEscape()
		{
			canBackEscape = true;
		}

		public static void DisableBackEscape()
		{
			Instance.InternalDisableBackEscape();
		}

		private void InternalDisableBackEscape()
		{
			canBackEscape = false;
		}

		public static void AddWidget(IWidget widget)
		{
			Instance.InternalAddWidget(widget);
		}

		private void InternalAddWidget(IWidget widget)
		{
			Widgets.Add(widget);
		}

		public static void RemoveWidget(IWidget widget)
		{
			Instance.InternalRemoveWidget(widget);
		}

		private void InternalRemoveWidget(IWidget widget)
		{
			Widgets.Remove(widget);
		}
		#endregion

		private void Start()
		{
			InternalUpdateWidget();
		}

		protected virtual void OnAwake() { }

		private void OnDestroy()
		{
			Instance = null;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				if (canBackEscape)
				{
					Back();
				}
			}

			OnUpdate();
		}

		protected virtual void OnUpdate() { }

		public static void Back()
		{
			Instance.InternalBack();
		}

		private void InternalBack()
		{
			if (popupStack.Count > 0)
			{
				if (popupStack.Peek() is IBackOverride backOverride)
				{
					backOverride.Back(null);
				}
				else
				{
					Pop(null);
				}
			}
			else if (menuStack.Count > 0)
			{
				string currentPath = menuStack.Peek();
				if (Menus[currentPath.Split('/')[0]] is IBackOverride backOverride)
				{
					backOverride.Back(currentPath);
				}
				else
				{
					Pop(null);
				}
			}
			else
			{
				BackOnRoot();
			}
		}

		protected virtual void BackOnRoot()
		{

		}
	}
}