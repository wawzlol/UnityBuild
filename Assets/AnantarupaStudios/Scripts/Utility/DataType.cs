using System;

namespace AnantarupaStudios.Utility
{
	[AttributeUsage(AttributeTargets.All)]
	public class DataType : Attribute
	{
		public Type Value;
		
		public DataType(Type value)
		{
			this.Value = value;
		}
	}
}
