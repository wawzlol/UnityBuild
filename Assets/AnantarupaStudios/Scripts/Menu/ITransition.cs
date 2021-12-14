using System.Threading.Tasks;

namespace AnantarupaStudios.Menu
{
	public interface ITransition
	{
		Task In();
		Task Out();
	}
}