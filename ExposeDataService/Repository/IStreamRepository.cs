using DataService.Models;

namespace ExposeDataService.Repository
{
    public interface IStreamRepository
    {
        IEnumerable<MachineStream> GetMachineStreams();
        MachineStream GetProductByID(int id);
        void DeleteMachineStream(int id);
    }
}
