using System.Threading.Tasks;

namespace AnantarupaStudios.Menu
{
	public interface IPopupController
	{
		string Path { get; }
		int Priority { get; }
		/// <summary>
		/// Called from PopupStack when self is not the highest Priority
		/// </summary>
		void Hide();
		/// <summary>
		/// Called from PopupStack when self is the highest Priority
		/// </summary>
		void Unhide();
		/// <summary>
		/// Called from MenuManager Pop
		/// </summary>
		void Close();
	}

	public interface IPopupController<T> : IPopupController
	{
		Task<T> Show(params object[] data);
	}
}