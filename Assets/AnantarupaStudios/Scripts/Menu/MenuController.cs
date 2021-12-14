using AnantarupaStudios.Menu.Transition;
using UnityEngine;

namespace AnantarupaStudios.Menu
{
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
    public abstract class MenuController : MonoBehaviour, IMenuController
    {
        public abstract string Path { get; }
        protected virtual bool ReportPathOnShow { get; } = true;
        public virtual bool HidePreviousMenu { get; } = true;

        protected Canvas canvas;
        protected CanvasGroup canvasGroup;
        private TransitionState transitionState;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            transitionState = gameObject.AddComponent<TransitionState>();
            OnAwake();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnShow(string path, params object[] data) { }
        protected virtual void OnShown(string path, params object[] data) { }
        protected virtual void OnDisabled(string path) { }
        protected virtual void OnEnabled(string path) { }
        protected virtual void OnHiding(string path) { }
        protected virtual void OnHidden(string path) { }
        protected virtual void OnUnhidden(string path) { }
        protected virtual void OnClosing(string path) { }
        protected virtual void OnClosed(string path) { }

        async void IMenuController.Show(string path, params object[] data)
        {
            OnShow(path, data);

            if (ReportPathOnShow) MenuManager.PushMenu(Path, HidePreviousMenu);

            gameObject.SetActive(true);
            canvas.enabled = true;

            bool transitionSuccess = await transitionState.In();
            if (this == null) return;
            if (!transitionSuccess)
            {
                gameObject.SetActive(transitionState.ShouldActive);
                canvas.enabled = transitionState.ShouldActive;
            }

            canvasGroup.blocksRaycasts = true;

            OnShown(path, data);
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

        async void IMenuController.Hide(string path)
        {
            OnHiding(path);

            canvasGroup.blocksRaycasts = false;

            bool transitionSuccess = await transitionState.Out();
            if (this == null) return;
            if (transitionSuccess)
            {
                canvas.enabled = false;
            }
            else
            {
                canvas.enabled = transitionState.ShouldActive;
            }

            OnHidden(path);
        }

        async void IMenuController.Unhide(string path)
        {
            canvas.enabled = true;

            bool transitionSuccess = await transitionState.In();
            if (this == null) return;
            if (transitionSuccess)
            {
                canvasGroup.blocksRaycasts = true;
				gameObject.SetActive(true);
			}
            else
            {
                canvasGroup.blocksRaycasts = transitionState.ShouldActive;
				gameObject.SetActive(transitionState.ShouldActive);
			}

            OnUnhidden(path);
        }

        async void IMenuController.Close(string path)
        {
            OnClosing(path);

            canvasGroup.blocksRaycasts = false;

            bool transitionSuccess = await transitionState.Out();
            if (this == null) return;
            if (transitionSuccess)
            {
                canvas.enabled = false;
                gameObject.SetActive(false);
            }
            else
            {
                canvas.enabled = transitionState.ShouldActive;
                gameObject.SetActive(transitionState.ShouldActive);
            }

            OnClosed(path);
        }
    }
}
