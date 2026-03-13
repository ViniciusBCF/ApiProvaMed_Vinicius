using ApiMedTest.Data.Repositories.Interfaces;
using ApiMedTest.Domain._Helper;
using ApiMedTest.Domain.Entities;
using ApiMedTest.Service.Notifications.Interfaces;
using ApiMedTest.Service.Results;
using ApiMedTest.Service.Results.Enums;
using ApiMedTest.Service.Service.Base;
using ApiMedTest.Service.Service.Interfaces;
using ApiMedTest.Service.ViewModels;
using AutoMapper;

namespace ApiMedTest.Service.Service
{
    public class ContatoService : BaseService, IContatoService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;

        public ContatoService(
            IContatoRepository contatoRepository,
            INotificador notificador,
            IMapper mapper) : base(notificador)
        {
            _contatoRepository = contatoRepository;
            _mapper = mapper;
            _notificador = notificador;
        }

        public async Task<Result<IEnumerable<ContatoViewModel>>> ObterTodosAtivosAsync()
        {
            var result = new Result<IEnumerable<ContatoViewModel>>();

            try
            {
                var lista = _mapper.Map<List<ContatoViewModel>>(
                    await _contatoRepository.ObterTodosAtivosAsync());

                if (lista.Any())
                {
                    result.Data = lista;
                    result.StatusCode = StatusCodeResultEnum.Ok;
                    result.Message = [Constantes.RetornandoContatosMsg(lista.Count)];
                }
                else
                {
                    result.StatusCode = StatusCodeResultEnum.NoContent;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = StatusCodeResultEnum.InternalServerError;
                result.IsValid = false;
                result.Message = [ex.Message];
            }

            return result;
        }

        public async Task<Result<ContatoViewModel>> ObterAtivoPorIdAsync(Guid id)
        {
            var result = new Result<ContatoViewModel>();

            try
            {
                var contato = await _contatoRepository.ObterAtivoPorIdAsync(id);

                if (contato is null)
                {
                    var contatoExistente = await _contatoRepository.ObterPorIdAsync(id);
                    if (contatoExistente is not null)
                    {
                        result.IsValid = false;
                        result.StatusCode = StatusCodeResultEnum.Conflict;
                        result.Message = [Constantes.ContatoInativoErrorMsg];
                    }
                    else
                    {
                        result.IsValid = false;
                        result.StatusCode = StatusCodeResultEnum.NotFound;
                        result.Message = [Constantes.ContatoNaoEncontradoErrorMsg];
                    }
                }
                else
                {
                    result.Data = _mapper.Map<ContatoViewModel>(contato);
                    result.StatusCode = StatusCodeResultEnum.Ok;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = StatusCodeResultEnum.InternalServerError;
                result.IsValid = false;
                result.Message = [ex.Message];
            }

            return result;
        }

        public async Task<Result<ContatoViewModel>> AdicionarAsync(ContatoViewModel contato)
        {
            var erros = Contato.Validar(
                contato.NomeContato,
                contato.DataNascimento,
                contato.Sexo);

            if (erros.Length != 0)
            {
                Notificar(erros);

                return new Result<ContatoViewModel>
                {
                    Data = contato,
                    IsValid = false,
                    StatusCode = StatusCodeResultEnum.BadRequest,
                    Message = _notificador.ObterMensagensNotificacoes()
                };
            }

            var entidade = new Contato(
                contato.NomeContato,
                contato.DataNascimento,
                contato.Sexo);
            entidade.Ativar();

            var contatoAdicionado = await _contatoRepository.AdicionarAsync(entidade);

            return new Result<ContatoViewModel>
            {
                Data = _mapper.Map<ContatoViewModel>(contatoAdicionado),
                IsValid = true,
                StatusCode = StatusCodeResultEnum.Created
            };
        }

        public async Task<Result<ContatoViewModel>> AtualizarAtivoAsync(Guid id, ContatoViewModel contato)
        {
            try
            {
                var entidade = await _contatoRepository.ObterAtivoPorIdParaEdicaoAsync(id);

                if (entidade is null)
                {
                    var contatoExistente = await _contatoRepository.ObterPorIdParaEdicaoAsync(id);
                    if (contatoExistente is not null)
                    {
                        return new Result<ContatoViewModel>
                        {
                            Data = _mapper.Map<ContatoViewModel>(contatoExistente),
                            IsValid = false,
                            StatusCode = StatusCodeResultEnum.Conflict,
                            Message = [Constantes.ContatoInativoErrorMsg]
                        };
                    }

                    return new Result<ContatoViewModel>
                    {
                        IsValid = false,
                        StatusCode = StatusCodeResultEnum.NotFound,
                        Message = [Constantes.ContatoNaoEncontradoAtivoErrorMsg]
                    };
                }

                var erros = entidade.AtualizarDados(
                    contato.NomeContato,
                    contato.DataNascimento,
                    contato.Sexo);

                if (erros.Length != 0)
                {
                    Notificar(erros);

                    return new Result<ContatoViewModel>
                    {
                        Data = contato,
                        IsValid = false,
                        StatusCode = StatusCodeResultEnum.BadRequest,
                        Message = _notificador.ObterMensagensNotificacoes()
                    };
                }

                await _contatoRepository.AtualizarAsync(entidade);

                return new Result<ContatoViewModel>
                {
                    Data = _mapper.Map<ContatoViewModel>(entidade),
                    StatusCode = StatusCodeResultEnum.Ok
                };
            }
            catch (Exception ex)
            {
                return new Result<ContatoViewModel>
                {
                    IsValid = false,
                    StatusCode = StatusCodeResultEnum.InternalServerError,
                    Message = [ex.Message]
                };
            }
        }

        public async Task<Result<ContatoViewModel>> AtivarAsync(Guid id)
        {
            try
            {
                var entidade = await _contatoRepository.ObterPorIdParaEdicaoAsync(id);

                if (entidade is null)
                {
                    return new Result<ContatoViewModel>
                    {
                        IsValid = false,
                        StatusCode = StatusCodeResultEnum.NotFound,
                        Message = [Constantes.ContatoNaoEncontradoErrorMsg]
                    };
                }

                entidade.Ativar();
                await _contatoRepository.AtualizarAsync(entidade);

                return new Result<ContatoViewModel>
                {
                    Data = _mapper.Map<ContatoViewModel>(entidade),
                    StatusCode = StatusCodeResultEnum.Ok,
                    Message = [Constantes.ContatoAtivadoMsg]
                };
            }
            catch (Exception ex)
            {
                return new Result<ContatoViewModel>
                {
                    IsValid = false,
                    StatusCode = StatusCodeResultEnum.InternalServerError,
                    Message = [ex.Message]
                };
            }
        }

        public async Task<Result<ContatoViewModel>> DesativarAsync(Guid id)
        {
            try
            {
                var entidade = await _contatoRepository.ObterAtivoPorIdParaEdicaoAsync(id);

                if (entidade is null)
                {
                    return new Result<ContatoViewModel>
                    {
                        IsValid = false,
                        StatusCode = StatusCodeResultEnum.NotFound,
                        Message = [Constantes.ContatoNaoEncontradoAtivoErrorMsg]
                    };
                }

                entidade.Desativar();
                await _contatoRepository.AtualizarAsync(entidade);

                return new Result<ContatoViewModel>
                {
                    Data = _mapper.Map<ContatoViewModel>(entidade),
                    StatusCode = StatusCodeResultEnum.Ok,
                    Message = [Constantes.ContatoDesativadoMsg]
                };
            }
            catch (Exception ex)
            {
                return new Result<ContatoViewModel>
                {
                    IsValid = false,
                    StatusCode = StatusCodeResultEnum.InternalServerError,
                    Message = [ex.Message]
                };
            }
        }

        public async Task<Result<ContatoViewModel>> RemoverAsync(Guid id)
        {
            try
            {
                var entidade = await _contatoRepository.ObterPorIdParaEdicaoAsync(id);

                if (entidade is null)
                {
                    return new Result<ContatoViewModel>
                    {
                        IsValid = false,
                        StatusCode = StatusCodeResultEnum.NotFound,
                        Message = [Constantes.ContatoNaoEncontradoErrorMsg]
                    };
                }

                await _contatoRepository.RemoverAsync(id);

                return new Result<ContatoViewModel>
                {
                    Data = _mapper.Map<ContatoViewModel>(entidade),
                    StatusCode = StatusCodeResultEnum.Ok,
                    Message = [Constantes.ContatoRemovidoMsg]
                };
            }
            catch (Exception ex)
            {
                return new Result<ContatoViewModel>
                {
                    IsValid = false,
                    StatusCode = StatusCodeResultEnum.InternalServerError,
                    Message = [ex.Message]
                };
            }
        }

        public void Dispose()
        {
            _contatoRepository?.Dispose();
        }
    }
}
