using System.ComponentModel.DataAnnotations;

namespace TechConnect.Models
{
    public class Palestrante
    {
        public int Id { get; set; }
        [Required]
        public string NomeCompleto { get; set; }
        [Required]
        public string Empresa { get; set; }
        public string? Cargo { get; set; }
        [Required]
        public string Especialidade { get; set; }
        [Required]
        public string Biografia { get; set; }
        public string? Foto { get; set; }
        public ICollection<EventoPalestrante> EventoPalestrantes { get; set; } = new List<EventoPalestrante>();
    }
}
