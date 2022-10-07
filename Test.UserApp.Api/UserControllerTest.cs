using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserApp.Api.Controllers;
using UserApp.Api.Models;
using UserApp.Api.Services.Queries.UserGetAll;
using UserApp.Api.Services.Queries.UserGetById;
using UserApp.Infrastructure.Repositories;

namespace Test.UserApp.Api
{
    public class UserControllerTest
    {
        private readonly UserController userController;
        private readonly Mock<ILogger<UserController>> logger;
        private readonly Mock<IMediator> mediator;

        public UserControllerTest()
        {
            mediator = new Mock<IMediator>();
            logger = new Mock<ILogger<UserController>>();
            userController = new UserController( mediator.Object, logger.Object );
        }
        [Fact]
        public async void AllUsersTest()
        {
            UserGetAllQuery query = new UserGetAllQuery();
            mediator.Setup(
                m => m.Send( query, It.IsAny<CancellationToken>() )
            ).ReturnsAsync( (IEnumerable<UserResponseModel>) new List<UserResponseModel>()
            {
                new UserResponseModel(){ Id = Guid.NewGuid().ToString(), UserName = "Api", Email="Api@gmail.com", LastLoginTime = DateTime.Now },
                new UserResponseModel(){ Id = Guid.NewGuid().ToString(), UserName = "Bpi", Email="Bpi@gmail.com", LastLoginTime = DateTime.Now },
                new UserResponseModel(){ Id = Guid.NewGuid().ToString(), UserName = "Cpi", Email="Cpi@gmail.com", LastLoginTime = DateTime.Now }
            } );

            IActionResult result = await userController.Index();
            Assert.IsAssignableFrom<IActionResult>( result );
        }

        [Fact]
        public async void UserGetByIdTest()
        {
            string id = Guid.NewGuid().ToString();

            UserGetByIdQuery query = new UserGetByIdQuery( id );
            mediator.Setup(
                m => m.Send( query, It.IsAny<CancellationToken>() )
            ).ReturnsAsync( new UserResponseModel()
            {
                Id = id,
                UserName = "Api",
                Email = "Api@gmail.com",
                LastLoginTime = DateTime.Now
            }
            );

            IActionResult result = await userController.GetById( id );
            Assert.IsAssignableFrom<IActionResult>( result );
        }
    }
}