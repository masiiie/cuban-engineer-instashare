namespace InstaShare.Domain.Repositories;

public interface ICrudBaseRepository<T>
{
	void Create(T entity);
	void Update(T entity);
	void Remove(T entity);
	Task<IEnumerable<T>> GetAllAsync(bool IncludeRelation = false);
	Task<T> GetByIdAsync(long id, bool IncludeRelation = false);
}