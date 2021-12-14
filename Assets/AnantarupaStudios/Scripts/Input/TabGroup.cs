using System.Collections.Generic;
using UnityEngine;

namespace AnantarupaStudios.Input
{
	public class TabGroup : MonoBehaviour
	{
		private readonly List<TabButton> tabButtons = new List<TabButton>();

		public event System.Action<string> TabSelected;
		public event System.Action<string> TabPressed;

		private void Awake()
		{
			foreach (Transform child in transform)
			{
				TabButton tabButton = child.GetComponent<TabButton>();
				if (tabButton)
				{
					tabButton.ButtonUp += OnButtonPressed;
					tabButtons.Add(tabButton);
				}
			}
		}

		public void SetTab(string path)
		{
			TabSelected?.Invoke(path);
			SetSelected(path);
		}

		public void SetSelected(string path)
		{
			for (int i = 0; i < tabButtons.Count; i++)
			{
				tabButtons[i].Selected = tabButtons[i].Path == path;
				if (tabButtons[i].Path == path)
				{
					TabName tabName = GetComponentInChildren<TabName>();
					if (tabName)
					{
						tabName.SetName(path);
						int targetIndex = tabButtons[i].transform.GetSiblingIndex() + 1;
						if (tabName.transform.GetSiblingIndex() < targetIndex)
						{
							targetIndex--;
						}
						tabName.transform.SetSiblingIndex(targetIndex);
					}
				}
			}
		}

		private void OnButtonPressed(string path)
		{
			TabPressed?.Invoke(path);
			SetTab(path);
		}

		private void OnDestroy()
		{
			TabSelected = null;
			TabPressed = null;
		}
	}
}