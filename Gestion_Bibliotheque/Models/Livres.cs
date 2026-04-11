using System.ComponentModel.DataAnnotations;

namespace Gestion_Bibliotheque.Models
{
    public class Livres
    {
        public int Id { get; set; } // Clé primaire automatique
        [MaxLength(200)]
        public string Titre { get; set; } = string.Empty;
        [MaxLength(150)]
        public string Auteur { get; set; } = string.Empty;
        [MaxLength(150)]
        public string categories { get; set; } = string.Empty;
        public bool EstEmprunte { get; set; } = false; 
        public string? Emprunteur { get; set; }        
    }
}
