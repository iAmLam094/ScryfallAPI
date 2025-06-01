using Microsoft.EntityFrameworkCore;
using ScryfallData;
using ScryfallData.Model;

namespace ScryfallAPI.Utilities
{
	public class AuthenticateUser
	{
		private ScryfallContext _context;
		private ILogger<AuthenticateUser>? _logger;
				
		public AuthenticateUser(ScryfallContext context, 
								ILogger<AuthenticateUser>? logger = null)
		{
			_context = context;
			_logger = logger;	
		}

	

		public async Task<User> AuthenticateUsers(string email)
		{
			User newUser = new User
			{
				Email = ""
			};


			var searchingForUser = _context.Users
								   .Select(u => u)
								   .Include(u => u.Favorites)
								   .ToList();
			
			if(!searchingForUser.Select(u => u.Email).Contains(email)) 
			{
				newUser.Email = email;
				await _context.Users.AddAsync(newUser);
				await _context.SaveChangesAsync();
				_logger.LogInformation($"User {newUser.Email} didn't exist, added to DB");
			}

			else
				searchingForUser.ForEach(u =>
				{
					if (u.Email == email)
					{
						newUser.Email = u.Email;
						newUser.Id = u.Id;
						newUser.Favorites = u.Favorites;
					}
				});

			return newUser;
			

		}
	}

}
