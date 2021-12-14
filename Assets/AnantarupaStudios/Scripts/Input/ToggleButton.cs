using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnantarupaStudios.Input
{
	public class ToggleButton : MonoBehaviour, IPointerDownHandler
	{
		private IToggleButtonView buttonView;

		public bool Value { get; private set; }
		public event Action<bool> ValueChanged;

		public void Awake()
		{
			if (TryGetComponent(out buttonView))
			{
				buttonView.SetView(Value);
			}
		}

		public void SetValue(bool value = true)
		{
			Value = value;
			ValueChanged?.Invoke(Value);

			if (buttonView != null)
			{
				buttonView.SetView(value);
			}
		}

#region IPointerHandler
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			Value = !Value;
			ValueChanged?.Invoke(Value);

			if (buttonView != null)
			{
				buttonView.SetView(Value);
			}
		}
#endregion

		protected void OnDestroy()
		{
			ValueChanged = null;
		}
	}
}