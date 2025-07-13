using HotelApp.Models;

namespace HotelApp.Interface.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Hotel> Hotels { get; }
        IGenericRepository<City> Cities { get; }
        IGenericRepository<Country> Countries { get; }
        Task<int> SaveAsync();
    }

}
