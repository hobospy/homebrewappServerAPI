using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace homebrewappServerAPI.Tests
{
    public class BrewServiceTests
    {
        private Brew testBrew = new Brew {  ID = 6000,
                                            Name = "TestBrew",
                                            BrewDate = new System.DateTime(),
                                            ABV = 5.5 };

        #region ListAsync
        [Test]
        public async Task ListAsync_ByDefault_CallsListAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.ListAsync();

            mockRepository.Verify(m => m.ListAsync(), Times.Once());
        }

        [Test]
        public async Task ListAsync_ByDefault_ReturnsList()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.ListAsync();

            Assert.IsNotNull(response);
        }
        #endregion

        #region GetAsync
        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithValidID_CallsFindIdAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.GetAsync(testBrew.ID);

            mockRepository.Verify(m => m.FindByIdAsync(testBrew.ID), Times.Once());
        }

        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithValidID_ReturnsSuccessTrue()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(testBrew);
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.GetAsync(1000);

            Assert.IsTrue(response.Success);
        }

        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithValidID_ReturnsBrew()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(testBrew);
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.GetAsync(1000);

            Assert.IsNotNull(response.Brew);
            Assert.AreEqual(testBrew, response.Brew);
        }

        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithInvalidID_ThrowsBadRequestException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.GetAsync(testBrew.ID + 1));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        #endregion

        #region SaveAsync
        [Test]
        public async Task SaveAsync_CalledWithValidData_CallsAddAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.SaveAsync(testBrew);

            mockRepository.Verify(m => m.AddAsync(It.IsAny<Brew>()), Times.Once());
        }

        [Test]
        public async Task SaveAsync_CalledWithValidData_ReturnsSuccessIsTrue()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync(testBrew);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task SaveAsync_CalledWithValidData_ReturnsBrew()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync(testBrew);

            Assert.AreEqual(testBrew, response.Brew);
        }

        [Test]
        public async Task SaveAsync_CalledWithNullBrew_ThrowsBadRequestException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.SaveAsync((Brew)null));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        #endregion

        #region UpdateAsync
        [Test]
        public async Task UpdateAsync_CalledWithValidData_CallsFindByIDOnce()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task UpdateAsync_CalledWithValidData_CallsUpdateOnce()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            mockRepository.Verify(m => m.Update(It.IsAny<Brew>()), Times.Once());
        }

        [Test]
        public async Task UpdateAsync_CalledWithValidData_ReturnsSuccessTrue()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task UpdateAsync_CalledWithValidData_ReturnsUpdateBrew()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            Assert.AreEqual(updatedTestBrew.ID, response.Brew.ID);
            StringAssert.AreEqualIgnoringCase(updatedTestBrew.Name, response.Brew.Name);
        }

        [Test]
        public async Task UpdateAsync_CalledWithInvalidData_ThrowsBadRequestException()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID + 1,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Test]
        public async Task UpdateAsync_UponTimeOut_ThrowsBadRequestException()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID))).Throws(new TimeoutException());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        #endregion

        #region PatchAsync
        [Test]
        public async Task PatchAsync_CalledWithValidData_CallsFindByIDOnce()
        {
            var testPatch = new JsonPatchDocument<Brew>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);
            testPatch.Replace(b => b.Name, "TestBrew - Updated");

            var response = await brewService.PatchAsync(testBrew.ID, testPatch);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task PatchAsync_CalledWithValidData_CallsUpdateOnce()
        {
            var testPatch = new JsonPatchDocument<Brew>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);
            testPatch.Replace(b => b.Name, "TestBrew - Updated");

            var response = await brewService.PatchAsync(testBrew.ID, testPatch);

            mockRepository.Verify(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)), Times.Once());
        }

        [Test]
        public async Task PatchAsync_CalledWithValidData_ReturnsSuccessTrue()
        {
            var testPatch = new JsonPatchDocument<Brew>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);
            testPatch.Replace(b => b.Name, "TestBrew - Updated");

            var response = await brewService.PatchAsync(testBrew.ID, testPatch);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task PatchAsync_CalledWithValidData_ReturnsUpdatedBrew()
        {
            var newName = "TestBrew - Updated";
            var updatedTestBrew = new Brew
            {
                ID = testBrew.ID,
                Name = newName,
                BrewDate = testBrew.BrewDate,
                ABV = testBrew.ABV
            };
            var testPatch = new JsonPatchDocument<Brew>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);
            testPatch.Replace(b => b.Name, newName);

            var response = await brewService.PatchAsync(testBrew.ID, testPatch);

            Assert.AreEqual(updatedTestBrew.ID, response.Brew.ID);
            StringAssert.AreEqualIgnoringCase(updatedTestBrew.Name, response.Brew.Name);
        }

        [Test]
        public async Task PatchAsync_CalledWithNullPatch_ReturnsBadRequest()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.PatchAsync(testBrew.ID, (JsonPatchDocument<Brew>)null));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Test]
        public async Task PatchAsync_CalledWithInvalidID_ReturnsBadRequest()
        {
            var testPatch = new JsonPatchDocument<Brew>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);
            testPatch.Replace(b => b.Name, "TestBrew - Updated");

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.PatchAsync(testBrew.ID + 1, testPatch));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Test]
        public async Task PatchAsync_UponTimeOut_ReturnsBadRequest()
        {
            var testPatch = new JsonPatchDocument<Brew>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID))).Throws(new TimeoutException());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);
            testPatch.Replace(b => b.Name, "TestBrew - Updated");

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.PatchAsync(testBrew.ID, testPatch));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        #endregion

        #region DeleteAsync
        [Test]
        public async Task DeleteAsync_CalledWithValidID_CallsFindByIDOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task DeleteAsync_CalledWithValidID_CallsRemoveOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID);

            mockRepository.Verify(m => m.Remove(It.IsAny<Brew>()), Times.Once());
        }

        [Test]
        public async Task DeleteAsync_CalledWithValidID_ReturnsSuccessTrue()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task DeleteAsync_CalledWithValidID_ReturnsDeletedBrew()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID);

            Assert.AreEqual(testBrew.ID, response.Brew.ID);
        }

        [Test]
        public async Task DeleteAsync_CalledWithInvalidID_ThrowsBadRequestException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            //mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.DeleteAsync(testBrew.ID + 1));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }

        [Test]
        public async Task DeleteAsync_RaisesKnownException_ThrowsBadRequestException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID))).Throws(new UnauthorizedAccessException());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await brewService.DeleteAsync(testBrew.ID));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        #endregion
    }
}