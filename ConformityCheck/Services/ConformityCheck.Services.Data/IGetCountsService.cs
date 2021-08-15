namespace ConformityCheck.Services.Data
{
    using System.Threading.Tasks;

    using ConformityCheck.Services.Data.Models;

    public interface IGetCountsService
    {
        // 1. Use the View Model
        // 2. Create Dto -> View Model
        // 3. Turles....(int a, int b, string c, bool d...)
        // TODO - make it async
        Task<CountsDto> GetCounts();
    }
}
