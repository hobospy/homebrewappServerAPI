using AutoMapper;
using homebrewAppServerAPI.Controllers;
using homebrewAppServerAPI.Domain.Services;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace homebrewappServerAPI.Tests
{
    public class BrewControllerTests
    {
        [Test]
        public async Task GetAllBrewsAsync_ByDefault_CallsListAsyncOnce()
        {
            var mockService = new Mock<IBrewService>();
            mockService.Setup(m => m.ListAsync());
            var mockMapper = new Mock<IMapper>();
            var brewController = new BrewController(mockService.Object, mockMapper.Object);

            await brewController.GetAllBrewsAsync();

            mockService.Verify(m => m.ListAsync(), Times.Once());
        }

    }
}
