using System.Collections.Generic;

namespace AnantarupaStudios.Menu
{
	public class PopupStack
	{
		[Sirenix.OdinInspector.ShowInInspector]
		private readonly List<IPopupController> stack = new List<IPopupController>();

		public int Count => stack.Count;

		public IPopupController Peek()
		{
			return stack[0];
		}

		public IPopupController Pop()
		{
			IPopupController top = stack[0];
			stack.RemoveAt(0);
			if (stack.Count > 0)
			{
				stack[0].Unhide();
			}
			return top;
		}

		public void Push(IPopupController popup)
		{
			if (stack.Count == 0)
			{
				stack.Insert(0, popup);
				return;
			}
			else if (popup.Priority >= stack[0].Priority)
			{
				stack[0].Hide();
				stack.Insert(0, popup);
				return;
			}
			popup.Hide();
			for (int i = 1; i < stack.Count; i++)
			{
				if (popup.Priority >= stack[i].Priority)
				{
					stack.Insert(i, popup);
					return;
				}
			}
			stack.Add(popup);
			return;
		}
	}
}