using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly StoreContext _context;

    public GenericRepository(StoreContext context)
    {
        _context = context;
    }
    
    public async Task<T> GetByIdAsync(int id)
    {
        T entity = await _context.Set<T>().FindAsync(id);

        return entity;
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        List<T> entities = await _context.Set<T>().ToListAsync();

        return entities;
    }
}