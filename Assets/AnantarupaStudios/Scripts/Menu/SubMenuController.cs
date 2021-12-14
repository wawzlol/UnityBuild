using UnityEngine;

namespace AnantarupaStudios.Menu
{
	[RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
	public abstract class SubMenuController : MonoBehaviour, ISubMenuController
	{
		public abstract string Path { get; }
		public virtual bool HidePreviousMenu { get; } = true;

		private Canvas canvas;
		private CanvasGroup canvasGroup;

		private void Awake()
		{
			canvas = GetComponent<Canvas>();
			canvas.enabled = false;
			canvasGroup = GetComponent<CanvasGroup>();
			canvasGroup.blocksRaycasts = false;
			OnAwake();
		}

		protected virtual void OnAwake() { }
		protected virtual void OnShow(string path, params object[] data) { }
		protected virtual void OnDisabled(string path) { }
		protected virtual void OnEnabled(string path) { }
		protected virtual void OnHiding(string path) { }
		protected virtual void OnHidden(string path) { }
		protected virtual void OnUnhidden(string path) { }
		protected virtual void OnClosed(string path) { }
		protected virtual void OnClosing(string path) { }

		public void Show(string path, params object[] data)
		{
			if (path == Path)
			{
				MenuManager.PushMenu(path);
			}
			OnShow(path, data);
			gameObject.SetActive(true);
			canvas.enabled = true;
			canvasGroup.blocksRaycasts = true;
		}

		public void Disable(string path)
		{
			canvasGroup.blocksRaycasts = false;
			OnDisabled(path);
		}

		public void Enable(string path)
		{
			canvasGroup.blocksRaycasts = true;
			OnEnabled(path);
		}

		public void Hiding(string path)
		{
			OnHiding(path);
		}

		public void Hide(string path)
		{
			canvasGroup.blocksRaycasts = false;
			canvas.enabled = false;
			OnHidden(path);
		}

		public void Unhide(string path)
		{
			canvasGroup.blocksRaycasts = true;
			canvas.enabled = true;
			OnUnhidden(path);
		}

		public void Closing(string path)
		{
			OnClosing(path);
		}

		public void Close(string path)
		{
			canvasGroup.blocksRaycasts = false;
			canvas.enabled = false;
			gameObject.SetActive(false);
			OnClosed(path);
		}
	}
}