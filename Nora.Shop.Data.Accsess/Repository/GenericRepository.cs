using Microsoft.EntityFrameworkCore;
using Nora.Shop.Core.Interfaces;
using Nora.Shop.DataAccess.Context;

namespace Nora.Shop.DataAccess.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly NoraShopContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(NoraShopContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public List<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id)!;

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity is null)
            {
                return;
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
