using ApiMedTest.Service.Results;
using ApiMedTest.Service.ViewModels;

namespace ApiMedTest.Service.Service.Interfaces
{
    public interface IContatoService : IDisposable
    {
        Task<Result<IEnumerable<ContatoViewModel>>> ObterTodosAtivosAsync();
        Task<Result<ContatoViewModel>> ObterAtivoPorIdAsync(Guid id);
        Task<Result<ContatoViewModel>> AdicionarAsync(ContatoViewModel contato);
        Task<Result<ContatoViewModel>> AtualizarAtivoAsync(Guid id, ContatoViewModel contato);
        Task<Result<ContatoViewModel>> AtivarAsync(Guid id);
        Task<Result<ContatoViewModel>> DesativarAsync(Guid id);
        Task<Result<ContatoViewModel>> RemoverAsync(Guid id);
    }
}
