using UniConnect.Enums;

namespace UniConnect.DTOs.Auth
{
    public class RegisterDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoUsuario Tipo { get; set; }

        // Dados opcionais para Aluno
        public string? Curso { get; set; }
        public int? Semestre { get; set; }

        // Dados opcionais para Professor
        public string? Departamento { get; set; }
        public string? Titulacao { get; set; }

        // Dados opcionais para Admin
        public AdminEnum? Cargo { get; set; }
    }
}
