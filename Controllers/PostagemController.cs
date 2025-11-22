using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UniConnect.Data;
using UniConnect.DTOs.Postagem;
using UniConnect.Models;

namespace UniConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostagemController : ControllerBase
    {
        private readonly DataContext _context;

        public PostagemController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Postagem
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPostagens()
        {
            var postagens = await _context.Postagens.ToListAsync();

            var response = postagens.Select(p => new PostagemResponseDTO
            {
                Id = p.Id,
                Conteudo = p.Conteudo,
                ArquivoUrl = p.ArquivoUrl,
                DataPublicacao = p.DataPublicacao,
                DataAtualizacao = p.DataAtualizacao,
                UsuarioId = p.UsuarioId,
                ComunidadeTematicaId = p.ComunidadeTematicaId
            });

            return Ok(response);
        }

        // GET: api/Postagem/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostagem(int id)
        {
            var postagem = await _context.Postagens.FirstOrDefaultAsync(x => x.Id == id);

            if (postagem == null)
                return NotFound(new { Message = $"Postagem com Id={id} não encontrada." });

            var response = new PostagemResponseDTO
            {
                Id = postagem.Id,
                Conteudo = postagem.Conteudo,
                ArquivoUrl = postagem.ArquivoUrl,
                DataPublicacao = postagem.DataPublicacao,
                DataAtualizacao = postagem.DataAtualizacao,
                UsuarioId = postagem.UsuarioId,
                ComunidadeTematicaId = postagem.ComunidadeTematicaId
            };

            return Ok(response);
        }

        // POST: api/Postagem
        [HttpPost]
        public async Task<IActionResult> CreatePostagem(PostagemCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "O corpo da requisição é inválido." });

            var userId = int.Parse(User.FindFirst("id")!.Value);

            var postagem = new Postagem
            {
                Conteudo = dto.Conteudo,
                ArquivoUrl = dto.ArquivoUrl,
                UsuarioId = userId,
                ComunidadeTematicaId = dto.ComunidadeTematicaId,
                DataPublicacao = DateTime.UtcNow
            };

            _context.Postagens.Add(postagem);
            await _context.SaveChangesAsync();

            var response = new PostagemResponseDTO
            {
                Id = postagem.Id,
                Conteudo = postagem.Conteudo,
                ArquivoUrl = postagem.ArquivoUrl,
                DataPublicacao = postagem.DataPublicacao,
                DataAtualizacao = postagem.DataAtualizacao,
                UsuarioId = postagem.UsuarioId,
                ComunidadeTematicaId = postagem.ComunidadeTematicaId
            };

            return CreatedAtAction(nameof(GetPostagem), new { id = postagem.Id }, response);
        }

        // PUT: api/Postagem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostagem(int id, PostagemUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "O corpo da requisição é inválido." });

            var postagem = await _context.Postagens.FirstOrDefaultAsync(x => x.Id == id);

            if (postagem == null)
                return NotFound(new { Message = $"Postagem com Id={id} não encontrada." });

            var userId = int.Parse(User.FindFirst("id")!.Value);
            if (postagem.UsuarioId != userId)
                return Forbid();

            postagem.Conteudo = dto.Conteudo;
            postagem.ArquivoUrl = dto.ArquivoUrl;
            postagem.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Postagem atualizada com sucesso.",
                Updated = postagem
            });
        }

        // DELETE: api/Postagem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostagem(int id)
        {
            var postagem = await _context.Postagens.FirstOrDefaultAsync(x => x.Id == id);

            if (postagem == null)
                return NotFound(new { Message = $"Postagem com Id={id} não encontrada." });

            var userId = int.Parse(User.FindFirst("id")!.Value);
            if (postagem.UsuarioId != userId)
                return Forbid();

            _context.Postagens.Remove(postagem);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Postagem removida com sucesso." });
        }
    }
}
