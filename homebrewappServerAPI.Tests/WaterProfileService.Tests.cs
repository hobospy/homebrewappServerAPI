using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace homebrewappServerAPI.Tests
{
    public class WaterProfileServiceTests
    {
        #region ListAsync
        [Test]
        public async Task ListAsync_ByDefault_CallsListAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IWaterProfileRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var waterProfileService = new WaterProfileService(mockRepository.Object, mockUnitOfWork.Object);

            await waterProfileService.ListAsync();

            mockRepository.Verify(m => m.ListAsync(), Times.Once());
        }

        [Test]
        public async Task ListAsync_ByDefault_ReturnsList()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IWaterProfileRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var waterProfileService = new WaterProfileService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await waterProfileService.ListAsync();

            Assert.IsNotNull(response);
        }
        #endregion
    }
}
