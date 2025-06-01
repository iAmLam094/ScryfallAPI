namespace ScryfallData.Model
{
	public class User
	{
		public int Id { get; set; }
		public required string Email { get; set; }
		public ICollection<FavoriteCards>? Favorites { get; set; } = new List<FavoriteCards>();
	}
}
