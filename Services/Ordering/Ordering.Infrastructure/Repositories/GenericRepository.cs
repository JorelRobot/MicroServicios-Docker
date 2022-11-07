using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly OrderContext orderContext;

        public GenericRepository(OrderContext orderContext) 
        {
            this.orderContext = orderContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await orderContext.AddAsync(entity);
            await orderContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            orderContext.Set<T>().Remove(entity);
            await orderContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await orderContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
            => await orderContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(int offset, int limit, 
            Expression<Func<T, bool>> predicate, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, 
            params string[] includeStrings)
        {
            /// Obtenemos la consulta dinamica
            IQueryable<T> query = orderContext.Set<T>();

            /// Especificamos la paginacion
            query = query.Skip(offset).Take(limit);

            /// Espesificamos los include (join)
            query = includeStrings.Aggregate(query, (current, itemInclude) => current.Include(itemInclude));

            /// Agragamos el filtro si es que hay
            if (predicate is not null) query = query.Where(predicate);

            /// Retornamos la lista ordenada si es que se puso ordeby
            if (orderBy is not null) return await orderBy(query).ToListAsync();

            /// Retornamos el resultado
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            /*
             * First o FirstOrDefault son como un SELECT TOP 1
             *
             * Estos se usan comunmente cuando queremos el primer dato no
             * importa que haya mas
             */
            /*
            var entity = await orderContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            var entity = await orderContext.Set<T>().FirstAsync(x => x.Id == id);
            */

            /*
             * Single o SingleOrDefault son como un SELECT TOP 2
             * 
             * Son comunmente usados cuando busquemos datos que queremos 
             * asegurarnos que siempre sean unicos con la convinacion que 
             * estemos mandano
             */
            /*
            var entity = await orderContext.Set<T>().SingleOrDefaultAsync(x => x.Id == id);

            var entity = await orderContext.Set<T>().SingleAsync(x => x.Id == id);
            */

            /*
             * Find se susa cuando se busca por llave primaria
             */
            var entity = await orderContext.Set<T>().FindAsync(id);

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            orderContext.Entry(entity).State = EntityState.Modified;
            await orderContext.SaveChangesAsync();

            return entity;
        }
    }
}
