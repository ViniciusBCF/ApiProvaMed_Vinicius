using ApiMedTest.Controllers.Base;
using ApiMedTest.Service.DTOs;
using ApiMedTest.Service.Notifications.Interfaces;
using ApiMedTest.Service.Service.Interfaces;
using ApiMedTest.Service.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiMedTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : BaseController
    {
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;

        public ContatosController(
            IContatoService contatoService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _contatoService = contatoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosAtivos()
        {
            return GetIActionResult(await _contatoService.ObterTodosAtivosAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterAtivoPorId([FromRoute] Guid id)
        {
            return GetIActionResult(await _contatoService.ObterAtivoPorIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] ContatoDTO contato)
        {
            return GetIActionResult(await _contatoService.AdicionarAsync(
                _mapper.Map<ContatoViewModel>(contato)));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] ContatoDTO contato)
        {
            return GetIActionResult(await _contatoService.AtualizarAtivoAsync(
                id,
                _mapper.Map<ContatoViewModel>(contato)));
        }

        [HttpPatch("{id:guid}/ativar")]
        public async Task<IActionResult> Ativar([FromRoute] Guid id)
        {
            return GetIActionResult(await _contatoService.AtivarAsync(id));
        }

        [HttpPatch("{id:guid}/desativar")]
        public async Task<IActionResult> Desativar([FromRoute] Guid id)
        {
            return GetIActionResult(await _contatoService.DesativarAsync(id));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remover([FromRoute] Guid id)
        {
            return GetIActionResult(await _contatoService.RemoverAsync(id));
        }
    }
}
