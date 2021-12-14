using System;
using System.Collections.Generic;
using System.Linq;

namespace AnantarupaStudios.Utility
{
	public static class EnumHelper<T> where T : Enum
	{
		static EnumHelper()
		{
			Dict = Enum.GetNames(typeof(T)).ToDictionary(x => x, x => (T)Enum.Parse(typeof(T), x), StringComparer.OrdinalIgnoreCase);
		}

		private static readonly Dictionary<string, T> Dict;

		public static T Convert(string value)
		{
			return Dict.TryGetValue(value, out T result) ? result : (Dict[value] = (T)Enum.Parse(typeof(T), value));
		}
	}

	public static class EnumHelper
	{
		public static string GetName<T>(this T value) where T : Enum
		{
			return Enum.GetName(typeof(T), value);
		}
	}
}