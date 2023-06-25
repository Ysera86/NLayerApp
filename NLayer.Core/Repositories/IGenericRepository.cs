using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();

        /// <summary>
        /// productRepository.Where(x => x.Id == 5).OrderBy... yazabilirim ondan IQueryable , ToList,ToListAsync yazdığım an memoryde birleştirilmiş sorgu ile gider dbden çeker
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>

        /// <summary>
        /// IQueryable: tolist demeden dbye gtmez, daha performanslı
        /// .linq yazıp durabilirim : productRepository.Where(x => x.Id == 5).OrderBy......> ne zaman ki productRepository.Where(x => x.Id == 5).OrderBy..ToListAsync() dedim o zaman dbden çeker. hazır order edilmiş şekilde alır datayı
        /// Eğer List yaparsam IQueryable yerine o zaman daha .Where esnasında gidip datayı çeker , memorye alır öyle order yapar,.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Where(Expression<Func<T,bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// memorye eklenecek entitiy ekliyor. EfCore
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(T entity); 
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// burada sadece state = modified yapıyor efcore, uzun süren bir işlem olmadığından efcore da update ve delete için asenkron metodu yok
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity); 
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
