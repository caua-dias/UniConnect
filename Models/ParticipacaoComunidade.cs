using UniConnect.Enums;
namespace UniConnect.Models
{
    public class ParticipacaoComunidade
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int ComunidadeTematicaId { get; set; }
        public ComunidadeTematica ComunidadeTematica { get; set; }

        public TipoUsuario Tipo { get; set; }
        public DateTime DataEntrada { get; set; } = DateTime.Now;
    }
}
