using System.Threading.Tasks;

public interface IAwaitable
{
	Task Task { get; }
}
