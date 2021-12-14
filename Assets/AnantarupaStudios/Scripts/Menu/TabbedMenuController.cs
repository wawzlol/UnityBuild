using AnantarupaStudios.Input;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace AnantarupaStudios.Menu
{
	public abstract class TabbedMenuController : MenuController
	{
		[Required, SerializeField] protected TabGroup tabGroup;

		[ShowInInspector]
		protected readonly Dictionary<string, ISubMenuController> subMenus = new Dictionary<string, ISubMenuController>();

		protected override void OnAwake()
		{
			foreach (Transform child in transform)
			{
				ISubMenuController subMenu = child.GetComponent<ISubMenuController>();
				if (!(subMenu is null))
				{
					subMenus[subMenu.Path] = subMenu;
				}
			}
			tabGroup.TabSelected += TabSet;
		}

		private void OnDestroy()
		{
			subMenus.Clear();
		}

		protected override void OnShow(string path, params object[] data)
		{
			if (path == Path)
			{
				tabGroup.SetSelected("");
			}
			else
			{
				tabGroup.SetSelected(path.Remove(0, Path.Length + 1).Split('/')[0]);
				subMenus[path].Show(path, data);
			}
		}

		protected override void OnEnabled(string path)
		{
			if (path != Path)
			{
				subMenus[path].Enable(path);
			}
		}

		protected override void OnDisabled(string path)
		{
			if (path != Path)
			{
				subMenus[path].Disable(path);
			}
		}

		protected override void OnHiding(string path)
		{
			if(path != Path)
			{
				subMenus[path].Hiding(path);
			}
		}

		protected override void OnHidden(string path)
		{
			if (path != Path)
			{
				subMenus[path].Hide(path);
			}
		}

		protected override void OnUnhidden(string path)
		{
			if (path != Path)
			{
				subMenus[path].Unhide(path);
			}
		}

		protected override void OnClosing(string path)
		{
			if(path != Path)
			{
				subMenus[path].Closing(path);
			}
		}

		protected override void OnClosed(string path)
		{
			if (path != Path)
			{
				subMenus[path].Close(path);
			}
		}

		protected virtual void TabSet(string path)
		{
			MenuManager.Pop(Path, false);
			Debug.Log($"{Path}/{path}");
			MenuManager.Show($"{Path}/{path}");
		}
	}
}
