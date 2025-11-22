namespace UniConnect.DTOs.Postagem
{
    public class PostagemResponseDTO
    {
        public int Id { get; set; }
        public string Conteudo { get; set; }
        public string? ArquivoUrl { get; set; }
        public DateTime DataPublicacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public int UsuarioId { get; set; }
        public int ComunidadeTematicaId { get; set; }
    }
}
