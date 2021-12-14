namespace AnantarupaStudios.Menu
{
	public interface IWidgetSubMenu
	{
		string Path { get; }
		void Show(string path, params object[] data);
		void Close(string path);
		void MenuChanged(string path);
	}
}