using System.ComponentModel;
using System.Reflection;

namespace AnantarupaStudios.Utility
{
	public static class EnumExtension
	{
		public static string GetDescriptionValue<T>(this T enumCode) where T : System.IConvertible
		{
			FieldInfo fi = enumCode.GetType().GetField(enumCode.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Description;
			else
				return enumCode.ToString();
		}

		public static DataType GetCustomType<T>(this T enumCode) where T : System.IConvertible
		{
			FieldInfo fi = enumCode.GetType().GetField(enumCode.ToString());

			DataType[] attributes = (DataType[])fi.GetCustomAttributes(typeof(DataType), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0];
			else
				return null;
		}
	}
}