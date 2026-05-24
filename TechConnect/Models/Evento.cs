using System.ComponentModel.DataAnnotations;

namespace TechConnect.Models
{
    public class Evento
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public string Horario { get; set; }
        [Required]
        public string Local { get; set; }
        [Required]
        public string Imagem { get; set; }
        public ICollection<EventoPalestrante> EventoPalestrantes { get; set; } = new List<EventoPalestrante>();
        public ICollection<EventoCategoria> EventoCategorias { get; set; } = new List<EventoCategoria>();
    }
}
