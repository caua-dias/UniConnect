namespace UniConnect.DTOs.Postagem
{
    public class PostagemCreateDTO
    {
        public string Conteudo { get; set; }
        public string? ArquivoUrl { get; set; }
        public int ComunidadeTematicaId { get; set; }
    }
}
