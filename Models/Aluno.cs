namespace UniConnect.Models
{
    public class Aluno : Usuario
    {
        public string Curso { get; set; }
        public int Semestre { get; set; }
    }
}
