using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Services;
using Moq;
using NUnit.Framework;
using System;
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
        [Category("ListAsync")]
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
        [Category("ListAsync")]
        public async Task WhenListAsyncIsCalled_ReturnsList()
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
        public async Task WhenGetAsyncIsCalled_CallsFindIdAsyncOnceForValidID()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.IsAny<int>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.GetAsync(1000);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        [Category("GetAsync")]
        public async Task WhenGetAsyncIsCalled_ReturnsSuccessTrueIfIDValid()
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
        public async Task WhenGetAsyncIsCalled_ReturnsBrewIfIDValid()
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
        public async Task WhenGetAsyncIsCalled_CallsFindIdAsyncOnceForInvalidID()
        {
            int BrewID = 1000;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == BrewID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.GetAsync(BrewID + 1);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        [Category("GetAsync")]
        public async Task WhenGetAsyncIsCalled_ReturnsSuccessFalseIfIDInvalid()
        {
            int BrewID = 1000;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == BrewID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.GetAsync(BrewID + 1);

            Assert.IsFalse(response.Success);
        }

        [Test]
        [Category("GetAsync")]
        public async Task WhenGetAsyncIsCalled_ReturnsNullIfIDInvalid()
        {
            int BrewID = 1000;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == BrewID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.GetAsync(BrewID + 1);

            Assert.IsNull(response.Brew);
        }

        [Test]
        [Category("GetAsync")]
        public async Task WhenGetAsyncIsCalled_ReturnsErrorMsgIfIDInvalid()
        {
            int BrewID = 1000;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == BrewID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.GetAsync(BrewID + 1);

            StringAssert.AreEqualIgnoringCase("Brew not found.", response.Message);
        }
        #endregion

        #region SaveAsync
        [Test]
        [Category("SaveAsync")]
        public async Task WhenSaveAsyncIsCalled_CallsAddAsyncOnceWithValidData()
        {
            var testBrew = new Brew { ID = 6000,
                                      Name = "TestBrew",
                                      BrewDate = new System.DateTime(),
                                      ABV = 5.5 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            await brewService.SaveAsync(testBrew);

            mockRepository.Verify(m => m.AddAsync(It.IsAny<Brew>()), Times.Once());
        }

        [Test]
        [Category("SaveAsync")]
        public async Task WhenSaveAsyncIsCalled_ReturnsSuccessIsTrueWithValidData()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync(testBrew);

            Assert.IsTrue(response.Success);
        }

        [Test]
        [Category("SaveAsync")]
        public async Task WhenSaveAsyncIsCalled_ReturnsBrewWithValidData()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync(testBrew);

            Assert.AreEqual(testBrew, response.Brew);
        }

        [Test]
        [Category("SaveAsync")]
        public async Task WhenSaveAsyncIsCalled_ReturnsSuccessFalseWithNullBrew()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync((Brew)null);

            Assert.IsFalse(response.Success);
        }

        [Test]
        [Category("SaveAsync")]
        public async Task WhenSaveAsyncIsCalled_ReturnsErrorMsgWithNullBrew()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>()));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync((Brew)null);

            StringAssert.AreEqualIgnoringCase("Cannot save invalid brew.", response.Message);
        }

        [Test]
        [Category("SaveAsync")]
        public async Task WhenSaveAsyncIsCalled_ReturnsErrorMsgWithNullBrew1()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.AddAsync(It.IsAny<Brew>())).ThrowsAsync(new TimeoutException());
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.SaveAsync(testBrew);

            Assert.IsFalse(string.IsNullOrEmpty(response.Message));
            StringAssert.StartsWith("An error occurred when saving the brew: ", response.Message);
        }
        #endregion

        #region UpdateAsync
        [Test]
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithValidData_CallsFindByIDOnce()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID)));
            mockRepository.Setup(m => m.Update(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithValidData_CallsUpdateOnce()
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
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithValidID_ReturnsSuccessTrue()
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
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithValidID_ReturnsUpdateBrew()
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
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithInvalidData_CallsFindByIDOnce()
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

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithInvalidData_DoesNotCallUpdate()
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

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            mockRepository.Verify(m => m.Update(It.IsAny<Brew>()), Times.Never());
        }

        [Test]
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithInvalidID_ReturnsSuccessFalse()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID + 1,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            Assert.AreEqual(false, response.Success);
        }

        [Test]
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncIsCalledWithInvalidID_ReturnsBrewNotFound()
        {
            var updatedTestBrew = new Brew { ID = testBrew.ID + 1,
                                             Name = "TestBrew - Updated",
                                             BrewDate = testBrew.BrewDate,
                                             ABV = testBrew.ABV };
            //TODO: Brittle test, need to get error string resource file
            var brewNotFound = "Brew not found.";

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            StringAssert.AreEqualIgnoringCase(brewNotFound, response.Message);
        }

        [Test]
        [Category("UpdateAsync")]
        public async Task WhenUpdateAsyncTimesOut_ReturnSuccessFalse()
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

            var response = await brewService.UpdateAsync(updatedTestBrew.ID, updatedTestBrew);

            Assert.IsFalse(string.IsNullOrEmpty(response.Message));
            StringAssert.StartsWith("An error occured when updating the brew: ", response.Message);
        }
        #endregion

        #region DeleteAsync
        [Test]
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithValidID_CallsFindByIDOnce()
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
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithValidID_CallsRemoveOnce()
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
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithValidID_ReturnsSuccessTrue()
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
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithValidID_ReturnsDeletedBrew()
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
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithInvalidID_CallsFindByIDOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID + 1);

            mockRepository.Verify(m => m.FindByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Test]
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithInvalidID_DoesNotCallRemove()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID + 1);

            mockRepository.Verify(m => m.Remove(It.IsAny<Brew>()), Times.Never());
        }

        [Test]
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithValidID_ReturnsSuccessFalse()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID + 1);

            Assert.IsFalse(response.Success);
        }

        [Test]
        [Category("DeleteAsync")]
        public async Task WhenDeleteAsyncIsCalledWithValidID_ReturnsBrewNotFound()
        {
            //TODO: Brittle test, need to get error string resource file
            var brewNotFound = "Brew not found.";

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID + 1);

            StringAssert.AreEqualIgnoringCase(brewNotFound, response.Message);
        }

        public async Task WhenDeleteAsyncTimesOut_ReturnSuccessFalse()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IBrewRepository>();
            mockRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testBrew.ID))).ReturnsAsync(testBrew);
            mockRepository.Setup(m => m.Remove(It.Is<Brew>(b => b.ID == testBrew.ID)));
            var brewService = new BrewService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await brewService.DeleteAsync(testBrew.ID + 1);

            Assert.IsFalse(response.Success);
            StringAssert.AreEqualIgnoringCase("An error occured when deleting the brew: ", response.Message);
        }
        #endregion
    }
}