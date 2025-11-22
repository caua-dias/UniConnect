using UniConnect.Enums;

namespace UniConnect.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoUsuario Tipo { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;


        public ICollection<ComunidadeTematica> ComunidadesCriadas { get; set; }
        public ICollection<ParticipacaoComunidade> Participacoes { get; set; }
        public ICollection<Postagem> Postagens { get; set; }
    }
}
