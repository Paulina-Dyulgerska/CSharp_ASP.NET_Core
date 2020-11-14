namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;

    public interface IService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllAsNoTracking<T>();
    }
}
