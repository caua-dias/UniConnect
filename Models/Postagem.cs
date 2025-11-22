namespace UniConnect.Models
{
    public class Postagem
    {
        public int Id { get; set; }
        public string Conteudo { get; set; }
        public string? ArquivoUrl { get; set; }
        public DateTime DataPublicacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int ComunidadeTematicaId { get; set; }
        public ComunidadeTematica ComunidadeTematica { get; set; }
    }
}
