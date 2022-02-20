using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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

    public async Task<T> GetEntityWithSpecification(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListEntitiesWithSpecification(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }
}