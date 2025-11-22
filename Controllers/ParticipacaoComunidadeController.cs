using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniConnect.Data;
using UniConnect.DTOs.Participacao;
using UniConnect.Enums;
using UniConnect.Models;

namespace UniConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParticipacaoComunidadeController : ControllerBase
    {
        private readonly DataContext _context;

        public ParticipacaoComunidadeController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ParticipacaoComunidade
        [HttpGet]
        public async Task<IActionResult> GetParticipacoes()
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var participacoes = await _context.ParticipacoesComunidades
                .Where(p => p.UsuarioId == userId)
                .ToListAsync();

            var dtoList = participacoes.Select(p => new ParticipacaoResponseDTO
            {
                Id = p.Id,
                UsuarioId = p.UsuarioId,
                ComunidadeTematicaId = p.ComunidadeTematicaId,
                Tipo = p.Tipo,
                DataEntrada = p.DataEntrada
            });

            return Ok(dtoList);
        }

        // GET: api/ParticipacaoComunidade/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipacao(int id)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var participacao = await _context.ParticipacoesComunidades.FindAsync(id);

            if (participacao == null)
                return NotFound(new { Message = $"Participação com Id={id} não encontrada." });

            if (participacao.UsuarioId != userId)
                return Forbid();

            var dto = new ParticipacaoResponseDTO
            {
                Id = participacao.Id,
                UsuarioId = participacao.UsuarioId,
                ComunidadeTematicaId = participacao.ComunidadeTematicaId,
                Tipo = participacao.Tipo,
                DataEntrada = participacao.DataEntrada
            };

            return Ok(dto);
        }

        // POST: api/ParticipacaoComunidade
        [HttpPost]
        public async Task<IActionResult> PostParticipacao(ParticipacaoCreateDTO dto)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);
            var tipoClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (tipoClaim == null)
                return Unauthorized(new { Message = "Token JWT inválido: claim 'role' ausente." });
            var userTipo = (TipoUsuario)Enum.Parse(typeof(TipoUsuario), tipoClaim!); ;

            if (dto == null)
                return BadRequest(new { Message = "O corpo da requisição é inválido." });

            var comunidade = await _context.ComunidadesTematicas.FindAsync(dto.ComunidadeTematicaId);
            if (comunidade == null)
                return BadRequest(new { Message = "Comunidade temática não encontrada." });

            var jaExiste = await _context.ParticipacoesComunidades
                .AnyAsync(x => x.UsuarioId == userId && x.ComunidadeTematicaId == dto.ComunidadeTematicaId);

            if (jaExiste)
                return BadRequest(new { Message = "Você já participa desta comunidade." });

            var nova = new ParticipacaoComunidade
            {
                UsuarioId = userId,
                ComunidadeTematicaId = dto.ComunidadeTematicaId,
                Tipo = userTipo
            };

            _context.ParticipacoesComunidades.Add(nova);
            await _context.SaveChangesAsync();

            var response = new ParticipacaoResponseDTO
            {
                Id = nova.Id,
                UsuarioId = nova.UsuarioId,
                ComunidadeTematicaId = nova.ComunidadeTematicaId,
                Tipo = nova.Tipo,
                DataEntrada = nova.DataEntrada
            };

            return CreatedAtAction(nameof(GetParticipacao), new { id = nova.Id }, response);
        }


        // DELETE: api/ParticipacaoComunidade/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipacao(int id)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var participacao = await _context.ParticipacoesComunidades.FindAsync(id);

            if (participacao == null)
                return NotFound(new { Message = $"Participação com Id={id} não encontrada." });

            if (participacao.UsuarioId != userId)
                return Forbid();

            _context.ParticipacoesComunidades.Remove(participacao);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Participação removida com sucesso." });
        }
    }
}
