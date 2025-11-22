using UniConnect.Enums;

namespace UniConnect.Models
{
    public class Admin : Usuario
    {
        public AdminEnum Cargo { get; set; }    
    }
}
