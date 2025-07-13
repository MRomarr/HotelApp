using HotelApp.Models;

namespace HotelApp.Interface.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Hotel> Hotels { get; }
        Task<int> SaveAsync();
    }

}
