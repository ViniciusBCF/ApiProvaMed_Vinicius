using ApiMedTest.Configuration;
using ApiMedTest.Data.Repositories.Interfaces;
using ApiMedTest.Domain._Helper;
using ApiMedTest.Domain.Entities;
using ApiMedTest.Domain.Entities.Enums;
using ApiMedTest.Service.Notifications;
using ApiMedTest.Service.Notifications.Interfaces;
using ApiMedTest.Service.Results.Enums;
using ApiMedTest.Service.Service;
using ApiMedTest.Service.Service.Interfaces;
using ApiMedTest.Service.ViewModels;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ApiMedTest.Tests.Contatos
{
    [Trait("Contato", nameof(ContatoService))]
    public class ContatoServiceTests
    {
        private readonly IContatoService _contatoService;
        private readonly Mock<IContatoRepository> _contatoRepository;

        public ContatoServiceTests()
        {
            _contatoRepository = new Mock<IContatoRepository>();
            INotificador notificador = new Notificador();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            }, NullLoggerFactory.Instance);
            IMapper mapper = mappingConfig.CreateMapper();

            _contatoService = new ContatoService(
                _contatoRepository.Object,
                notificador,
                mapper);
        }

        [Fact]
        public async Task AdicionarAsync_QuandoEntidadeValida_DeveRetornarCreated()
        {
            var viewModel = CriarContatoValido();
            _contatoRepository
                .Setup(r => r.AdicionarAsync(It.IsAny<Contato>()))
                .ReturnsAsync((Contato contato) => contato);

            var result = await _contatoService.AdicionarAsync(viewModel);

            result.IsValid.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Created);
            result.Data.Should().NotBeNull();
            result.Data!.StatusContato.Should().Be(StatusContato.Ativo);
        }

        [Fact]
        public async Task AdicionarAsync_QuandoEntidadeInvalida_DeveRetornarBadRequest()
        {
            var viewModel = CriarContatoInvalidoComIdadeZero();

            var result = await _contatoService.AdicionarAsync(viewModel);

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodeResultEnum.BadRequest);
            result.Message.Should().Contain(Constantes.IdadeInvalidaErrorMsg);
        }

        [Fact]
        public async Task ObterAtivoPorIdAsync_QuandoNaoEncontrado_DeveRetornarNotFound()
        {
            _contatoRepository.Setup(r => r.ObterAtivoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contato?)null);
            _contatoRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contato?)null);

            var result = await _contatoService.ObterAtivoPorIdAsync(Guid.NewGuid());

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodeResultEnum.NotFound);
            result.Message.Should().Contain(Constantes.ContatoNaoEncontradoErrorMsg);
        }

        [Fact]
        public async Task ObterAtivoPorIdAsync_QuandoContatoExisteMasEstaInativo_DeveRetornarConflict()
        {
            var entidade = CriarEntidadeValida();
            entidade.Desativar();
            _contatoRepository.Setup(r => r.ObterAtivoPorIdAsync(entidade.Id)).ReturnsAsync((Contato?)null);
            _contatoRepository.Setup(r => r.ObterPorIdAsync(entidade.Id)).ReturnsAsync(entidade);

            var result = await _contatoService.ObterAtivoPorIdAsync(entidade.Id);

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Conflict);
            result.Message.Should().Contain(Constantes.ContatoInativoErrorMsg);
        }

        [Fact]
        public async Task AtualizarAtivoAsync_QuandoEntidadeValida_DeveRetornarOk()
        {
            var entidade = CriarEntidadeValida();
            var viewModel = CriarContatoValido();

            _contatoRepository.Setup(r => r.ObterAtivoPorIdParaEdicaoAsync(entidade.Id)).ReturnsAsync(entidade);

            var result = await _contatoService.AtualizarAtivoAsync(entidade.Id, viewModel);

            result.IsValid.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Ok);
            result.Data!.NomeContato.Should().Be(viewModel.NomeContato);
        }

        [Fact]
        public async Task AtualizarAtivoAsync_QuandoContatoExisteMasEstaInativo_DeveRetornarConflict()
        {
            var entidade = CriarEntidadeValida();
            entidade.Desativar();
            var viewModel = CriarContatoValido();

            _contatoRepository.Setup(r => r.ObterAtivoPorIdParaEdicaoAsync(entidade.Id)).ReturnsAsync((Contato?)null);
            _contatoRepository.Setup(r => r.ObterPorIdParaEdicaoAsync(entidade.Id)).ReturnsAsync(entidade);

            var result = await _contatoService.AtualizarAtivoAsync(entidade.Id, viewModel);

            result.IsValid.Should().BeFalse();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Conflict);
            result.Message.Should().Contain(Constantes.ContatoInativoErrorMsg);
        }

        [Fact]
        public async Task AtivarAsync_QuandoContatoExiste_DeveRetornarOk()
        {
            var entidade = CriarEntidadeValida();
            entidade.Desativar();
            _contatoRepository.Setup(r => r.ObterPorIdParaEdicaoAsync(entidade.Id)).ReturnsAsync(entidade);

            var result = await _contatoService.AtivarAsync(entidade.Id);

            result.IsValid.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Ok);
            result.Data!.StatusContato.Should().Be(StatusContato.Ativo);
            result.Message.Should().Contain(Constantes.ContatoAtivadoMsg);
        }

        [Fact]
        public async Task DesativarAsync_QuandoContatoAtivoExiste_DeveRetornarOk()
        {
            var entidade = CriarEntidadeValida();
            _contatoRepository.Setup(r => r.ObterAtivoPorIdParaEdicaoAsync(entidade.Id)).ReturnsAsync(entidade);

            var result = await _contatoService.DesativarAsync(entidade.Id);

            result.IsValid.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Ok);
            result.Data!.StatusContato.Should().Be(StatusContato.Inativo);
            result.Message.Should().Contain(Constantes.ContatoDesativadoMsg);
        }

        [Fact]
        public async Task RemoverAsync_QuandoContatoExiste_DeveRetornarOk()
        {
            var entidade = CriarEntidadeValida();
            _contatoRepository.Setup(r => r.ObterPorIdParaEdicaoAsync(entidade.Id)).ReturnsAsync(entidade);

            var result = await _contatoService.RemoverAsync(entidade.Id);

            result.IsValid.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodeResultEnum.Ok);
            result.Message.Should().Contain(Constantes.ContatoRemovidoMsg);
        }

        [Fact]
        public async Task ObterTodosAtivosAsync_QuandoNaoHaRegistros_DeveRetornarNoContent()
        {
            _contatoRepository.Setup(r => r.ObterTodosAtivosAsync()).ReturnsAsync([]);

            var result = await _contatoService.ObterTodosAtivosAsync();

            result.StatusCode.Should().Be(StatusCodeResultEnum.NoContent);
        }

        private static ContatoViewModel CriarContatoValido()
        {
            return new ContatoViewModel
            {
                NomeContato = "Maria Santos",
                DataNascimento = DateTime.Today.AddYears(-30),
                Sexo = Sexo.Feminino
            };
        }

        private static ContatoViewModel CriarContatoInvalidoComIdadeZero()
        {
            return new ContatoViewModel
            {
                NomeContato = "Maria Santos",
                DataNascimento = DateTime.Today,
                Sexo = Sexo.Feminino
            };
        }

        private static Contato CriarEntidadeValida()
        {
            var entidade = new Contato(
                "Maria Santos",
                DateTime.Today.AddYears(-30),
                Sexo.Feminino);
            entidade.Ativar();
            return entidade;
        }
    }
}
