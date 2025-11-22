namespace UniConnect.Models
{
    public class ComunidadeTematica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        public ICollection<ParticipacaoComunidade> Participacoes { get; set; }
        public ICollection<Postagem> Postagens { get; set; }
    }
}
