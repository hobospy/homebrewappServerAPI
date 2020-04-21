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
    public class RecipeServiceTests
    {
        #region ListAsync
        [Test]
        public async Task ListAsync_ByDefault_CallsListAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRecipeRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var recipeService = new RecipeService(mockRepository.Object, mockUnitOfWork.Object);

            await recipeService.ListAsync();

            mockRepository.Verify(m => m.ListAsync(), Times.Once());
        }

        [Test]
        public async Task ListAsync_ByDefault_ReturnsList()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IRecipeRepository>();
            mockRepository.Setup(m => m.ListAsync());
            var recipeService = new RecipeService(mockRepository.Object, mockUnitOfWork.Object);

            var response = await recipeService.ListAsync();

            Assert.IsNotNull(response);
        }
        #endregion
    }
}
