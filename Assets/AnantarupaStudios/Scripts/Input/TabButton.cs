using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        [Required, SerializeField] private string path;

        protected Image inputArea;

        public event Action<string> ButtonDown;
        public event Action<string> ButtonUp;
        public event Action<string> ButtonExit;

        private bool _active = true;
        private bool _pressed = false;
        private bool _selected = false;

		private ITabButtonView buttonView;

		public string Path => path;

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
				if (buttonView != null)
				{
					buttonView.SetView(_selected, _active, _pressed);
				}
				inputArea.raycastTarget = _active;
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnSelectedChanged(value);
				if (buttonView != null)
				{
					buttonView.SetView(_selected, _active, _pressed);
				}
			}
        }

        protected virtual void OnSelectedChanged(bool value) { }

        public bool Pressed
        {
            get => _pressed;
            private set
            {
                _pressed = value;
                
				if (buttonView != null)
				{
					buttonView.SetView(_selected, _active, _pressed);
				}
			}
        }

        private void Awake()
        {
            inputArea = GetComponent<Image>();
			TryGetComponent(out buttonView);
		}

#region IPointerHandler
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!_active || _selected) return;

            Pressed = true;
            ButtonDown?.Invoke(path);
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (!_active || !_pressed) return;
			if (eventData.dragging) return;

			Pressed = false;
			ButtonUp?.Invoke(path);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!_active) return;

			Pressed = false;
            ButtonExit?.Invoke(path);
		}
#endregion
		private void OnDestroy()
        {
            ButtonDown = null;
            ButtonUp = null;
            ButtonExit = null;
        }


#if UNITY_EDITOR
		[ContextMenu("Get References")]
		public void GetReferences()
		{
			path = gameObject.name.ToLower();
		}
#endif
	}
}