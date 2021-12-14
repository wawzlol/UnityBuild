using System.Collections.Generic;
using UnityEngine;

namespace AnantarupaStudios.Utility
{
	public static class ScriptableObjectExtension
	{
		public static void ChangeListCount<T>(this ScriptableObject so, int Count, ref List<T> paramList)
		{
			int temp = Count - paramList.Count;
			if (temp != 0)
			{
				if (temp < 0)
				{
					paramList.RemoveRange(Count, Mathf.Abs(temp));
				}
				else
				{
					while (paramList.Count < Count)
					{
						paramList.Add(default);
					}
				}
			}
		}
	}
}