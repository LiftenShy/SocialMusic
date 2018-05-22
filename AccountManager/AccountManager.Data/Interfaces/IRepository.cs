using System.Collections.Generic;
using System.Linq;

namespace AccountManager.Data.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> Table { get; }

        IEnumerable<T> GetAll();

        T GetById(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
