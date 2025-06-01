using ScryfallAPI;
using Moq;
using ScryfallData.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static ScryfallAPI.Utilities.APIRetriever;
using ScryfallData;
namespace ScryfallTests
{
	[TestClass]
	public class ScryfallAPITests
	{
		[TestMethod]
		public void DataExists_InDbContext()
		{
			//Arrange

			var list = new List<User>()
			{
				new User
				{
					Id = 1,
					Email = "firstEmailMock"
				},
				new User
				{
					Id = 2,
					Email = "secondEmailMock"
				}

			}.AsQueryable();
			

			//Act
			var moqUsersSet = new Mock<DbSet<User>>();
			moqUsersSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(list.Provider);
			moqUsersSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(list.Expression);
			moqUsersSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(list.ElementType);
			moqUsersSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());

			var moqContext = new Mock<ScryfallContext>();
			moqContext.Setup(m => m.Users).Returns(moqUsersSet.Object);
			string emailToTest = "firstEmailMock";

			//Assert
			Assert.AreEqual(2, moqContext.Object.Users.Count());
			Assert.IsTrue(moqContext.Object.Users
										   .Select(u => u.Email == emailToTest)
										   .FirstOrDefault());
		}

		[TestMethod]
		public void Connection_ToAPIEnpoint_Check()
		{
			//Arrange
			var url = "https://api.scryfall.com/cards/search?q=b%3Ausg";
			using var client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "Accept");

			//Assert
			Assert.IsTrue(client.GetAsync(url).Result.IsSuccessStatusCode);

		}
	}
}