using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace homebrewappServerAPI.Tests
{
    public class RecipeServiceTests
    {
        private Recipe testRecipe = new Recipe
        {
            ID = 2000,
            Name = "TestRecipe",
            Description = "Description of the test recipe created for testing this code",
            Favourite = true,
            Type = ETypeOfBeer.ScotIrishAle
        };

        #region ListAsync
        [Test]
        public async Task ListAsync_ByDefault_CallsListAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIngredientRepository = new Mock<IIngredientRepository>();
            var mockRecipeRepository = new Mock<IRecipeRepository>();
            mockRecipeRepository.Setup(m => m.ListAsync());
            var recipeService = new RecipeService(mockRecipeRepository.Object, mockIngredientRepository.Object, mockUnitOfWork.Object);

            await recipeService.ListAsync();

            mockRecipeRepository.Verify(m => m.ListAsync(), Times.Once());
        }

        [Test]
        public async Task ListAsync_ByDefault_ReturnsList()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIngredientRepository = new Mock<IIngredientRepository>();
            var mockRecipeRepository = new Mock<IRecipeRepository>();
            mockRecipeRepository.Setup(m => m.ListAsync());
            var recipeService = new RecipeService(mockRecipeRepository.Object, mockIngredientRepository.Object, mockUnitOfWork.Object);

            var response = await recipeService.ListAsync();

            Assert.IsNotNull(response);
        }
        #endregion

        #region GetAsync
        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithValidID_CallsFindIdAsyncOnce()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIngredientRepository = new Mock<IIngredientRepository>();
            var mockRecipeRepository = new Mock<IRecipeRepository>();
            mockRecipeRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testRecipe.ID))).ReturnsAsync(testRecipe);
            var recipeService = new RecipeService(mockRecipeRepository.Object, mockIngredientRepository.Object, mockUnitOfWork.Object);

            await recipeService.GetAsync(testRecipe.ID);

            mockRecipeRepository.Verify(m => m.FindByIdAsync(testRecipe.ID), Times.Once());
        }

        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithValidID_ReturnsSuccessTrue()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIngredientRepository = new Mock<IIngredientRepository>();
            var mockRecipeRepository = new Mock<IRecipeRepository>();
            mockRecipeRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testRecipe.ID))).ReturnsAsync(testRecipe);
            var recipeService = new RecipeService(mockRecipeRepository.Object, mockIngredientRepository.Object, mockUnitOfWork.Object);

            var response = await recipeService.GetAsync(testRecipe.ID);

            Assert.IsTrue(response.Success);
        }

        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithValidID_ReturnsBrew()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIngredientRepository = new Mock<IIngredientRepository>();
            var mockRecipeRepository = new Mock<IRecipeRepository>();
            mockRecipeRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testRecipe.ID))).ReturnsAsync(testRecipe);
            var recipeService = new RecipeService(mockRecipeRepository.Object, mockIngredientRepository.Object, mockUnitOfWork.Object);

            var response = await recipeService.GetAsync(testRecipe.ID);

            Assert.IsNotNull(response.Recipe);
            Assert.AreEqual(testRecipe, response.Recipe);
        }

        [Test]
        [Category("GetAsync")]
        public async Task GetAsync_CalledWithInvalidID_ThrowsBadRequestException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockIngredientRepository = new Mock<IIngredientRepository>();
            var mockRecipeRepository = new Mock<IRecipeRepository>();
            mockRecipeRepository.Setup(m => m.FindByIdAsync(It.Is<int>(i => i == testRecipe.ID)));
            var recipeService = new RecipeService(mockRecipeRepository.Object, mockIngredientRepository.Object, mockUnitOfWork.Object);

            var ex = Assert.ThrowsAsync<homebrewAPIException>(async () => await recipeService.GetAsync(testRecipe.ID + 1));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        #endregion
    }
}
