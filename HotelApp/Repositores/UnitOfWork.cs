using HotelApp.Data;
using HotelApp.Interface.Repositories;
using HotelApp.Models;
using System;

namespace HotelApp.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<Hotel>? _Hotels;
        private IGenericRepository<City>? _Citys;
        private IGenericRepository<Country>? _Countries;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Hotel> Hotels
            => _Hotels ??= new GenericRepository<Hotel>(_context);
        public IGenericRepository<City> Cities
            => _Citys ??= new GenericRepository<City>(_context);
        public IGenericRepository<Country> Countries
            => _Countries ??= new GenericRepository<Country>(_context);


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
