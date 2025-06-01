
using Microsoft.EntityFrameworkCore;
using ScryfallData.Model;

namespace ScryfallData
{
    public class ScryfallContext : DbContext
    {
        public ScryfallContext(DbContextOptions options) : base(options) { }

        public ScryfallContext() { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsRequired();
		}

		
		#region DbSet
		public virtual DbSet<FavoriteCards> Favorites { get; set; }
        public virtual DbSet<User> Users { get; set; }
		#endregion
	}
}
