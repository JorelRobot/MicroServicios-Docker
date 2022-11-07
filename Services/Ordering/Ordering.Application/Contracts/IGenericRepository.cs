using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        /*
            Agregar
            Borrar
            Obtener datos
            Obtener todos con un filtro
            Obtener todos con un filtro, paginado, ordenamiento y Joins
            Obtener uno con un filtro
            Obtener por Id
            Actualizar
        */

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(int offset, int limit, Expression<Func<T, bool>> predicate, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, params string[] includeStrings);


    }

}

