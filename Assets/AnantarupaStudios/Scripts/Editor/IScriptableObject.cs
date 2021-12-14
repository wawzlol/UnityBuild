namespace AnantarupStudios.Editor
{
	public interface IScriptableObject
	{
		void ValueChanged();
		void ValueSet();
		void Load();
		void Save();
	}
}