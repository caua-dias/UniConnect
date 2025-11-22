using System.Text.Json.Serialization;
using UniConnect.Enums;

namespace UniConnect.DTOs.Auth
{
    public class RegisterResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoUsuario Tipo { get; set; }
        public DateTime DataCriacao { get; set; }

        // Dados opcionais para Aluno
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Curso { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Semestre { get; set; }

        // Dados opcionais para Professor
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Departamento { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Titulacao { get; set; }

        // Dados opcionais para Admin
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AdminEnum? Cargo { get; set; }
    }
}
