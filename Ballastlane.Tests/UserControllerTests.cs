using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;



namespace BallastLane.Tests 
{
    public class UserControllerTests
    {
        [Fact]
        public void Get_ReturnsListOfUsers()
        {
            // Arrange
            var mockRepo = new Mock<UserService>();
            mockRepo.Setup(repo => repo.GetAllUsersAsync()).Returns((Task<IEnumerable<UserEntity>>)GetTestUsers());
            var controller = new UserController(mockRepo.Object);

            // Act
            var result = controller.GetUsers();

            // Assert
            var actionResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<IEnumerable<UserEntity>>(actionResult.Value);
            Xunit.Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Post_CreatesNewUser()
        {
            // Arrange
            var mockRepo = new Mock<UserService>();
            var controller = new UserController(mockRepo.Object);
            var newUser = new UserEntity { Id = 4, Username = "New User" };

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var createdAtActionResult = Xunit.Assert.IsType<CreatedAtActionResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<UserEntity>(createdAtActionResult.Value);
            Xunit.Assert.Equal(newUser.Id, model.Id);
            Xunit.Assert.Equal(newUser.Username, model.Username);
        }

       

        private static IEnumerable<UserEntity> GetTestUsers()
        {
            return
            [
                new UserEntity { Id = 1, Username = "User 1" },
                new UserEntity { Id = 2, Username = "User 2" },
                new UserEntity { Id = 3, Username = "User 3" }
            ];
        }
    }
}
