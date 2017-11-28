using System.Threading.Tasks;

namespace TCC.ODBDriver.Commands
{
    public interface ICommand
    {
        Task<double> GetValue();
    }
}
