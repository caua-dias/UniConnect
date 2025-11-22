using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniConnect.Data;
using UniConnect.DTOs.Auth;
using UniConnect.Enums;
using UniConnect.Models;
using UniConnect.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace UniConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly TokenService _tokenService;

        public AuthController(DataContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/Auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterDTO dto)
        {
            var exists = await _context.Usuarios.AnyAsync(x => x.Email == dto.Email);
            if (exists)
                return Conflict(new { Message = "Email já cadastrado." });

            Usuario usuario;
            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            switch (dto.Tipo)
            {
                case TipoUsuario.Aluno:
                    usuario = new Aluno
                    {
                        Nome = dto.Nome,
                        Email = dto.Email,
                        Senha = hashed,
                        Tipo = dto.Tipo,
                        Curso = dto.Curso!,
                        Semestre = dto.Semestre ?? 1
                    };
                    break;

                case TipoUsuario.Professor:
                    usuario = new Professor
                    {
                        Nome = dto.Nome,
                        Email = dto.Email,
                        Senha = hashed,
                        Tipo = dto.Tipo,
                        Departamento = dto.Departamento!,
                        Titulacao = dto.Titulacao!
                    };
                    break;

                case TipoUsuario.Admin:
                    usuario = new Admin
                    {
                        Nome = dto.Nome,
                        Email = dto.Email,
                        Senha = hashed,
                        Tipo = dto.Tipo,
                        Cargo = dto.Cargo ?? AdminEnum.Secretario
                    };
                    break;

                default:
                    return BadRequest("Tipo de usuário inválido.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new RegisterResponseDTO
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

        // POST: api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (usuario == null)
                return Unauthorized("Credenciais inválidas.");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha);
            if (!valid)
                return Unauthorized("Credenciais inválidas.");

            var token = _tokenService.GenerateToken(
                usuario.Id,
                usuario.Email,
                usuario.Nome,
                usuario.Tipo.ToString()
            );

            return Ok(new LoginResponseDTO
            {
                Token = token,
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo,

                Curso = (usuario is Aluno al) ? al.Curso : null,
                Semestre = (usuario is Aluno al2) ? al2.Semestre : null,

                Departamento = (usuario is Professor pr) ? pr.Departamento : null,
                Titulacao = (usuario is Professor pr2) ? pr2.Titulacao : null,

                Cargo = (usuario is Admin ad) ? ad.Cargo : null
            });
        }
    }
}
