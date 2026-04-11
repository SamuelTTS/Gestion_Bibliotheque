using System.ComponentModel.DataAnnotations;

namespace Gestion_Bibliotheque.Models
{
    public class Users
    {
        public int Id { get; set; } // Clé primaire automatique

        [MaxLength(100)]
        public string name { get; set; }= string.Empty;
        [MaxLength(100)]
        public string email { get; set; }=string.Empty;
        public string password { get; set; }=string.Empty;
        [MaxLength(50)]
        public string role { get; set; }="lecteur";
        public DateTime DateInscription { get; set; } = DateTime.Now;
    }
}
