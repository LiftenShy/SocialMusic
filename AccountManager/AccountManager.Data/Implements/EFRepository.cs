using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Data.Implements
{
    public class EfRepository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _entities;
        private DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());
        public virtual IQueryable<T> Table => Entities;
        private string _errorMessage = string.Empty;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.Select(x => x).ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await Entities.FindAsync(id);
        }

        public async void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                Entities.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                
            }
        }

        public async void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception dbEx)
            {
                
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
