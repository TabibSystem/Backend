//
// using Microsoft.EntityFrameworkCore;
// using TabibApp.Application;
// using TabibApp.Infrastructure.Data;
//
// namespace TabibApp.Infrastructure.Repository
// {
//     public class BaseRepository<T> : IBaseRepository<T> where T : class
//     {
//         protected readonly AppDbContext _context;
//         protected readonly DbSet<T> _dbSet;
//
//         public BaseRepository(AppDbContext context)
//         {
//             _context = context;
//             _dbSet = _context.Set<T>();
//         }
//
//         public async Task<IEnumerable<T>> GetAll()
//         {
//             return await _dbSet.ToListAsync();
//         }
//
//         public async Task<T> GetById(int id)
//         {
//             return await _dbSet.FindAsync(id);
//         }
//
//         public async Task<T> Add(T entity)
//         {
//             await _dbSet.AddAsync(entity);
//             await _context.SaveChangesAsync();
//             return entity;
//         }
//
//         public async Task<T> Update(T entity)
//         {
//             _dbSet.Attach(entity);
//             _context.Entry(entity).State = EntityState.Modified;
//             await _context.SaveChangesAsync();
//             return entity;
//         }
//
//         public async Task<T> Delete(int id)
//         {
//             var entity = await _dbSet.FindAsync(id);
//             if (entity == null)
//             {
//                 return null;
//             }
//
//             _dbSet.Remove(entity);
//             await _context.SaveChangesAsync();
//             return entity;
//         }
//
//         public async Task<int> SaveChangesAsync()
//         {
//             return await _context.SaveChangesAsync();
//         }
//     }
// }