using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManager.Data.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> Table { get; }

        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
