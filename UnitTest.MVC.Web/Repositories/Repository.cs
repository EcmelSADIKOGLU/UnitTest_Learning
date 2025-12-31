
using Microsoft.EntityFrameworkCore;
using UnitTest.MVC.Web.Models;

namespace UnitTest.MVC.Web.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly UnitTestLearningDbContext _context;
        private readonly DbSet<T> _dbSet;   
        public Repository(UnitTestLearningDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var value = await _dbSet.ToListAsync();
            return value;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var value = await _dbSet.FindAsync([id]);
            return value;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
