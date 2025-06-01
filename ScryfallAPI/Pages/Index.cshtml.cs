using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScryfallData;
using ScryfallData.Model;

namespace ScryfallAPI.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
		public ScryfallContext _context { get; set; }
	
		public IndexModel(ScryfallContext context) 
		{
			_context = context;
		}

        public void OnGet()
        {

        }

		public User GetUser(int  id) 
		{
			var searching = _context.Users.Where(u => u.Id == id)
										  .Include(u => u.Favorites)
										  .First();

			_context.Entry<User>(searching).State = EntityState.Detached;
			
			return searching;
		}

		public void OnPostSend(string name, int pennyRank, string releasedDate)
		{
			using ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddConsole());
			ILogger logger = loggerFactory.CreateLogger<IndexModel>();
			
			var retrievingClaims = User.Claims.First(c => c.Type == "UserId");
			string returnUrl = "https://localhost:7224/index";
			
			var searchingForUser = GetUser(Convert.ToInt32(retrievingClaims.Value));

			FavoriteCards favoriteCards = new FavoriteCards()
			{
				IsFavorite = true,
				Name = name,
				PennyRank = pennyRank,
				ReleasedAt = releasedDate,
				UserId = Convert.ToInt32(retrievingClaims?.Value),
				User = searchingForUser
			};

			searchingForUser.Favorites = new List<FavoriteCards> { favoriteCards };
			favoriteCards.User.Favorites = searchingForUser.Favorites;

			_context.Entry(favoriteCards).State = EntityState.Modified;
			_context.Favorites.Add(favoriteCards);
			_context.SaveChanges();

			logger.LogInformation($"{favoriteCards.Name} marked as favorite in the db");
			

			Redirect(returnUrl);


		}

	}
}
