using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        /// TODO: Confoguration
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        /// Funcionalidad para poner en automatico los campos de auditoria
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastMidifiedBy = "User"; // TODO: Hasta  que tengamos ususarios lo obtendremos del token
                        break;
                    case EntityState.Added:
                        entry.Entity.CreateDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "User"; // TODO: Hasta  que tengamos ususarios lo obtendremos del token
                        break;

                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// Funcionalidad Multi-Tenencia
        public Guid TenantId { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var entityType = entity.ClrType;

                if (!typeof(IMultiTenant).IsAssignableFrom(entityType)) continue;

                var method = typeof(IMultiTenant).GetMethod(nameof(MultiTenantExpression),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
                    .MakeGenericMethod(entityType);

                var filter = method?.Invoke(null, new object[] { this })!;

                entity.SetQueryFilter((LambdaExpression)filter);

                entity.AddIndex(entity.FindDeclaredProperty(nameof(IMultiTenant.TenantId))!);

            }
        }

        private static LambdaExpression MultiTenantExpression<T>
            (OrderContext context) where T : EntityBase, IMultiTenant
        {
            Expression<Func<T, bool>> tenantFilter = x => x.TenantId == context.TenantId;
            return tenantFilter;
        }

    }
}
