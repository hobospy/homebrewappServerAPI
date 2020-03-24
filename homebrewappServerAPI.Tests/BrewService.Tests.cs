using AutoMapper;
using homebrewAppServerAPI.Controllers;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Persistence.Contexts;
using homebrewAppServerAPI.Persistence.Repositories;
using homebrewAppServerAPI.Resources;
using homebrewAppServerAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace homebrewappServerAPI.Tests
{
    public class BrewServiceTests
    {
        private static DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestingDB")
            .Options;

        private AppDbContext testDBContext = new AppDbContext(options);


        [Test]
        public async Task WhenListAsyncIsCalled_ItShouldCallListAsync()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.ListAsync();

            mockRepository.Verify(m => m.ListAsync(), Times.Once());
        }

        [Test]
        public async Task WhenGetAsyncIsCalled_ItShouldCallFindIdAsync()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.IsAny<int>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.GetAsync(1000);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsyncIsCalled_ItShouldCallAddAsync()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.SaveAsync(new Brew { ID = 6000, Name = "TestBrew", BrewDate = new System.DateTime(), ABV = 5.5 });

            mockRepository.Verify(m => m.AddAsync(It.IsAny<Brew>()), Times.Once());
        }

        [Test]
        public async Task WhenUpdateAsyncIsCalled_ItShouldCallUpdate()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.Update(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.UpdateAsync(1, new Brew { ID = 6000, Name = "TestBrew", BrewDate = new System.DateTime(), ABV = 5.5 });

            mockRepository.Verify(m => m.Update(It.IsAny<Brew>()), Times.Once());
        }

        [Test]
        public async Task WhenDeleteAsyncIsCalled_ItShouldCallRemove()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.Remove(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.DeleteAsync(1);

            mockRepository.Verify(m => m.Update(It.IsAny<Brew>()), Times.Once());
        }



        // Needs to be moved to the controller test class
        [Test]
        public async Task WhenGetAllBrewsAsyncIsCalled_ItShouldCallListAsync()
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