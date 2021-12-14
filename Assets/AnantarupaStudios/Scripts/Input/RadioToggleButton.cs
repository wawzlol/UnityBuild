using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnantarupaStudios.Input
{
	public class RadioToggleButton : MonoBehaviour, IPointerClickHandler
	{
		public int Index { get; private set; }
		public bool Value { get; private set; }

		private IToggleButtonView buttonView;

		public event Action<int> RadioPressed;

		public void Awake()
		{
			if (TryGetComponent(out buttonView))
			{
				buttonView.SetView(Value);
			}
		}

		private void OnDestroy()
		{
			RadioPressed = null;
		}

		#region IPointerHandler
		public void OnPointerClick(PointerEventData eventData)
		{
			RadioPressed?.Invoke(Index);
		}
		#endregion

		public void SetIndex(int index)
		{
			Index = index;
		}

		public void SetState(bool value)
		{
			Value = value;
			if (buttonView != null)
			{
				buttonView.SetView(Value);
			}
		}
	}
}