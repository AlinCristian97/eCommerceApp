using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int idl);
    Task<IReadOnlyList<T>> ListAllAsync();
}