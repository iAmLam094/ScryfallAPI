using System.ComponentModel.DataAnnotations.Schema;

namespace ScryfallData.Model
{
    public class FavoriteCards
    {
        public int Id { get; set; }
        public bool IsFavorite { get; set; } 
        public string? Name{ get; set; }
        public string? ReleasedAt { get; set; }
        public int PennyRank { get; set; }
        
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
