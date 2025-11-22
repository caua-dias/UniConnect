using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniConnect.Data;
using UniConnect.DTOs.Comunidade;
using UniConnect.Models;
using Microsoft.AspNetCore.Authorization;

namespace UniConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComunidadeTematicaController : ControllerBase
    {
        private readonly DataContext _context;

        public ComunidadeTematicaController(DataContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------------
        // GET ALL (público)
        // -------------------------------------------------------------
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var comunidades = await _context.ComunidadesTematicas
                .Select(c => new ComunidadeResponseDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    UsuarioId = c.UsuarioId,
                    DataCriacao = c.DataCriacao,
                    DataAtualizacao = c.DataAtualizacao
                })
                .ToListAsync();

            return Ok(comunidades);
        }

        // -------------------------------------------------------------
        // TOP communities
        // -------------------------------------------------------------
        [AllowAnonymous]
        [HttpGet("top")]
        public async Task<IActionResult> Top(int take = 5)
        {
            var top = await _context.ComunidadesTematicas
                .Select(c => new
                {
                    c.Id,
                    c.Nome,
                    Members = c.Participacoes.Count,
                    Posts = c.Postagens.Count
                })
                .OrderByDescending(x => x.Members)
                .ThenByDescending(x => x.Posts)
                .Take(take)
                .ToListAsync();

            return Ok(top);
        }

        // -------------------------------------------------------------
        // READ BY ID (público)
        // -------------------------------------------------------------
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadById(int id)
        {
            var comunidade = await _context.ComunidadesTematicas
                .Where(c => c.Id == id)
                .Select(c => new ComunidadeResponseDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    UsuarioId = c.UsuarioId,
                    DataCriacao = c.DataCriacao,
                    DataAtualizacao = c.DataAtualizacao
                })
                .FirstOrDefaultAsync();

            if (comunidade == null)
                return NotFound(new { Message = $"Comunidade com Id={id} não encontrada." });

            return Ok(comunidade);
        }

        // -------------------------------------------------------------
        // CREATE (pega id do usuário do token)
        // -------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComunidadeCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Corpo da requisição inválido.");

            var claimId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (claimId == null)
                return Unauthorized(new { Message = "Token sem id de usuário." });

            int userId = int.Parse(claimId);

            var nova = new ComunidadeTematica
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                UsuarioId = userId
            };

            _context.ComunidadesTematicas.Add(nova);
            await _context.SaveChangesAsync();

            var response = new ComunidadeResponseDTO
            {
                Id = nova.Id,
                Nome = nova.Nome,
                Descricao = nova.Descricao,
                UsuarioId = nova.UsuarioId,
                DataCriacao = nova.DataCriacao,
                DataAtualizacao = nova.DataAtualizacao
            };

            return CreatedAtAction(nameof(ReadById), new { id = nova.Id }, response);
        }

        // -------------------------------------------------------------
        // UPDATE (somente o criador pode alterar)
        // -------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ComunidadeUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest("Corpo da requisição inválido.");

            var comunidade = await _context.ComunidadesTematicas.FirstOrDefaultAsync(c => c.Id == id);
            if (comunidade == null)
                return NotFound(new { Message = $"Comunidade com Id={id} não encontrada." });

            // ID do usuário logado
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (claimId == null)
                return Unauthorized(new { Message = "Token sem id de usuário." });

            int userId = int.Parse(claimId);

            // Verifica se o usuário é dono
            if (comunidade.UsuarioId != userId)
                return Forbid("Somente o criador da comunidade pode editá-la.");

            comunidade.Nome = dto.Nome ?? comunidade.Nome;
            comunidade.Descricao = dto.Descricao ?? comunidade.Descricao;
            comunidade.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Comunidade atualizada com sucesso." });
        }

        // -------------------------------------------------------------
        // DELETE (somente o criador pode excluir)
        // -------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comunidade = await _context.ComunidadesTematicas.FirstOrDefaultAsync(c => c.Id == id);
            if (comunidade == null)
                return NotFound(new { Message = $"Comunidade com Id={id} não encontrada." });

            // ID do usuário logado
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (claimId == null)
                return Unauthorized(new { Message = "Token sem id de usuário." });

            int userId = int.Parse(claimId);

            if (comunidade.UsuarioId != userId)
                return Forbid("Somente o criador da comunidade pode removê-la.");

            _context.ComunidadesTematicas.Remove(comunidade);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Comunidade '{comunidade.Nome}' removida com sucesso." });
        }
    }
}
