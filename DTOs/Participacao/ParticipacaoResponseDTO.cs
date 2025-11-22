using UniConnect.Enums;

namespace UniConnect.DTOs.Participacao
{
    public class ParticipacaoResponseDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ComunidadeTematicaId { get; set; }
        public TipoUsuario Tipo { get; set; }
        public DateTime DataEntrada { get; set; }
    }
}
