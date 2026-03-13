using ApiMedTest.Data.Context;
using ApiMedTest.Data.Repositories.Interfaces.Base;
using ApiMedTest.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiMedTest.Data.Repositories.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly AppDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(AppDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity?> ObterPorIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodosAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<TEntity> AdicionarAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task AtualizarAsync(TEntity entity)
        {
            var entidadeLocal = DbSet.Local.FirstOrDefault(item => item.Id == entity.Id);
            if (entidadeLocal is not null && !ReferenceEquals(entidadeLocal, entity))
            {
                Db.Entry(entidadeLocal).State = EntityState.Detached;
            }

            Db.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public virtual async Task RemoverAsync(Guid id)
        {
            var entidadeLocal = DbSet.Local.FirstOrDefault(item => item.Id == id);
            if (entidadeLocal is not null)
            {
                DbSet.Remove(entidadeLocal);
            }
            else
            {
                var entidade = await DbSet.FindAsync(id);
                if (entidade is null)
                {
                    return;
                }

                DbSet.Remove(entidade);
            }

            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
