using System;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialMusic.Data.Abstract;
using SocialMusic.Models.EntityModels.BaseModels;

namespace SocialMusic.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SocialMusicContext _context;

        private DbSet<T> _entities;

        public EfRepository(SocialMusicContext context)
        {
            _context = context;
        }

        public async Task<T> GetById(int id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public virtual IQueryable<T> Table => Entities;

        private DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
