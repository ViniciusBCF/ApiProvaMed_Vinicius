using ApiMedTest.Data.Repositories.Interfaces.Base;
using ApiMedTest.Domain.Entities;

namespace ApiMedTest.Data.Repositories.Interfaces
{
    public interface IContatoRepository : IRepository<Contato>
    {
        Task<List<Contato>> ObterTodosAtivosAsync();
        Task<Contato?> ObterAtivoPorIdAsync(Guid id);
        Task<Contato?> ObterPorIdParaEdicaoAsync(Guid id);
        Task<Contato?> ObterAtivoPorIdParaEdicaoAsync(Guid id);
    }
}
