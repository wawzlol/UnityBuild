using UnityEngine;

namespace AnantarupaStudios.Input
{
	public class RadioButton : MonoBehaviour
	{
		private RadioToggleButton[] buttons;
		
		public int Value { get; private set; }
		public event System.Action<int> ValueChanged;

		public void Awake()
		{
			buttons = GetComponentsInChildren<RadioToggleButton>();
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].SetIndex(i);
				buttons[i].RadioPressed += OnRadioPressed;
				buttons[i].SetState(i == Value);
			}
			if (Value > buttons.Length)
			{
				Debug.LogError($"value out of range {Value} > {buttons.Length}");
			}
		}

		private void OnDestroy()
		{
			if (buttons != null)
			{
				for (int i = 0; i < buttons.Length; i++)
				{
					buttons[i].RadioPressed -= OnRadioPressed;
				}
			}
			ValueChanged = null;
		}

		public void SetValue(int value)
		{
			if (value < 0)
			{
				Debug.LogError($"value out of range {value} < 0");
				return;
			}

			if (buttons == null)
			{
				Value = value;
				return;
			}
			else if (value > buttons.Length)
			{
				Debug.LogError($"value out of range {value} > {buttons.Length}");
				return;
			}
			else
			{
				Value = value;
			}

			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].SetState(i == value);
			}
		}

		#region Event Handler
		private void OnRadioPressed(int value)
		{
			SetValue(value);

			ValueChanged?.Invoke(value);
		}
		#endregion
	}
}