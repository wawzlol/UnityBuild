namespace AnantarupaStudios.Menu
{
    public interface ISubMenuController : IMenuController
	{
		void Hiding(string path);
		void Closing(string path);
	}
}