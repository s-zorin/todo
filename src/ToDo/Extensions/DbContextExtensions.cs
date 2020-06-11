using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task<EntityEntry<T>> AddOrUpdateAsync<T>(this DbContext context, T entity) where T : class
        {
            var existingEntity = await GetExistingEntityAsync(context, entity);
            
            if (existingEntity == null)
            {
                return await AddAsync(context, entity);
            }
            else
            {
                return await UpdateAsync(context, existingEntity, entity);
            }
        }

        private static async Task<T?> GetExistingEntityAsync<T>(DbContext context, T entity) where T: class
        {
            var entityId = GetEntityId(context, entity);
            return await context.Set<T>().FindAsync(entityId); ;
        }

        private static object? GetEntityId(DbContext context, object entity)
        {
            var entry = context.Entry(entity);
            var idProperty = entry.Metadata.FindPrimaryKey().Properties.FirstOrDefault();
            return idProperty?.GetGetter().GetClrValue(entity);
        }

        private static Task<EntityEntry<T>> UpdateAsync<T>(DbContext context, T existingEntity, T entity) where T : class
        {
            var entry = context.Entry(existingEntity);
            entry.CurrentValues.SetValues(entity);
            return Task.FromResult(entry);
        }

        private static async Task<EntityEntry<T>> AddAsync<T>(DbContext context, T entity) where T : class
        {
            return await context.AddAsync(entity);
        }

    }
}