
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMusic.Data.Abstract
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<List<T>> GetAll();
        IQueryable<T> Table { get; }
    }
}
