using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UniConnect.Data;
using UniConnect.DTOs.Usuario;
using UniConnect.Enums;
using UniConnect.Models;

namespace UniConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _context;

        public UsuarioController(DataContext context)
        {
            _context = context;
        }

        // GET: api/usuario
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            return Ok(usuarios.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Tipo = u.Tipo,
                DataCriacao = u.DataCriacao,

                Curso = (u is Aluno al) ? al.Curso : null,
                Semestre = (u is Aluno al2) ? al2.Semestre : null,

                Departamento = (u is Professor pr) ? pr.Departamento : null,
                Titulacao = (u is Professor pr2) ? pr2.Titulacao : null,

                Cargo = (u is Admin ad) ? ad.Cargo : null

            }).ToList());
        }

        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { Message = $"Usuário com Id={id} não encontrado." });

            return Ok(new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo,
                DataCriacao = usuario.DataCriacao,

                Curso = (usuario is Aluno al) ? al.Curso : null,
                Semestre = (usuario is Aluno al2) ? al2.Semestre : null,

                Departamento = (usuario is Professor pr) ? pr.Departamento : null,
                Titulacao = (usuario is Professor pr2) ? pr2.Titulacao : null,

                Cargo = (usuario is Admin ad) ? ad.Cargo : null
            });
        }

        

        // PUT: api/usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UsuarioUpdateDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { Message = $"Usuário com Id={id} não encontrado." });

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Usuário atualizado com sucesso.",
                Updated = dto
            });
        }


        // PUT: api/usuario/alterar-senha/{id}
        [HttpPut("alterar-senha/{id}")]
        public async Task<IActionResult> AlterarSenha(int id, UsuarioSenhaUpdateDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { Message = $"Usuário com Id={id} não encontrado." });

            var valid = BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, usuario.Senha);
            if (!valid)
                return BadRequest(new { Message = "Senha atual incorreta." });

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Senha alterada com sucesso." });
        }

        // DELETE: api/Usuario/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var loggedUserId = int.Parse(User.FindFirst("id")!.Value);
            var loggedUserName = User.FindFirst("nome")!.Value;
            var loggedUserTipo = Enum.Parse<TipoUsuario>(User.FindFirst("tipo")!.Value);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            // Regra: só pode deletar se:
            // - For o próprio usuário OU
            // - For Admin
            if (loggedUserId != id && loggedUserTipo != TipoUsuario.Admin)
            {
                return Unauthorized($"{loggedUserName} não possui autorização para deletar o usuário de id {id}");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
