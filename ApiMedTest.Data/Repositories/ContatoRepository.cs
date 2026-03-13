using ApiMedTest.Data.Context;
using ApiMedTest.Data.Repositories.Base;
using ApiMedTest.Data.Repositories.Interfaces;
using ApiMedTest.Domain.Entities;
using ApiMedTest.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApiMedTest.Data.Repositories
{
    public class ContatoRepository : Repository<Contato>, IContatoRepository
    {
        public ContatoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Contato>> ObterTodosAtivosAsync()
        {
            return await DbSet
                .AsNoTracking()
                .Where(contato => contato.StatusContato == StatusContato.Ativo)
                .ToListAsync();
        }

        public async Task<Contato?> ObterAtivoPorIdAsync(Guid id)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(contato =>
                    contato.Id == id && contato.StatusContato == StatusContato.Ativo);
        }

        public async Task<Contato?> ObterPorIdParaEdicaoAsync(Guid id)
        {
            return await DbSet
                .FirstOrDefaultAsync(contato => contato.Id == id);
        }

        public async Task<Contato?> ObterAtivoPorIdParaEdicaoAsync(Guid id)
        {
            return await DbSet
                .FirstOrDefaultAsync(contato =>
                    contato.Id == id && contato.StatusContato == StatusContato.Ativo);
        }
    }
}
