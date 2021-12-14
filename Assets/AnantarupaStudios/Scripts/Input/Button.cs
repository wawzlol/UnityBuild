using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
	[RequireComponent(typeof(Image))]
	public class Button : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
	{
		private Image _inputArea;
		public event Action ButtonDown;
		public event Action ButtonUp;
		public event Action ButtonExit;

		private bool _active = true;
		private bool _pressed = false;

		protected bool Pressed => _pressed;

		private IButtonView buttonView;

		public Image InputArea => (_inputArea) ? _inputArea : (_inputArea = GetComponent<Image>());

		public void Awake()
		{
			TryGetComponent(out buttonView);
		}

		#region IPointerHandler
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (!_active) return;

			_pressed = true;
			ButtonDown?.Invoke();

			if (buttonView != null)
			{
				buttonView.SetView(_active, _pressed);
			}
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (!_active || !_pressed) return;
			if (eventData.dragging) return;

			_pressed = false;
			ButtonUp?.Invoke();

			if (buttonView != null)
			{
				buttonView.SetView(_active, _pressed);
			}
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (!_active) return;

			_pressed = false;
			ButtonExit?.Invoke();

			if (buttonView != null)
			{
				buttonView.SetView(_active, _pressed);
			}
		}
		#endregion

		public virtual void EnableInput()
		{
			_active = true;
			InputArea.raycastTarget = _active; // DEBT inputArea bisa null kalau belum awake. check null?

			if (buttonView != null)
			{
				buttonView.SetView(_active, _pressed);
			}
		}

		public virtual void DisableInput()
		{
			_active = false;
			InputArea.raycastTarget = _active;

			if (buttonView != null)
			{
				buttonView.SetView(_active, _pressed);
			}
		}

		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}

		protected virtual void OnDestroy()
		{
			ButtonDown = null;
			ButtonUp = null;
			ButtonExit = null;
		}
	}
}