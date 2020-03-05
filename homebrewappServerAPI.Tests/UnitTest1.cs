using AutoMapper;
using homebrewAppServerAPI.Controllers;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Resources;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace homebrewappServerAPI.Tests
{
    public class Tests
    {
        private BrewController _brewController;
        private Mock<List<BrewResource>> _moqBrewResourceList;
        private Mock<IBrewService> _moqBrewService;
        private Mock<IMapper> _moqMapper;

        [SetUp]
        public void Setup()
        {
            _moqBrewResourceList = new Mock<List<BrewResource>>();
            _moqBrewService = new Mock<IBrewService>();
            _moqMapper = new Mock<IMapper>();
            _brewController = new BrewController(_moqBrewService.Object, _moqMapper.Object);
        }

        [Test]
        public void BrewSummaryGet_ReturnsListOfBrews()
        {
            var brewName = "BrewTest1";
            var mockPosts = new List<BrewResource> {
                new BrewResource{   ABV = 5.5,
                                    BrewDate = new System.DateTime(),
                                    BrewFavourite = false,
                                    ID = 2000,
                                    Name = brewName,
                                    Recipe = new RecipeResource{ID = 5000,
                                                                Name = "Test Recipe 1",
                                                                Description = "An example brew recipe",
                                                                Type = ETypeOfBeer.IPA.ToString()},
                                                                TastingNotes = "Test tasting notes, very tasty" }
            };

            _moqBrewResourceList.Object.AddRange(mockPosts);

            var result = _brewController.GetAllBrewsAsync();

            Assert.IsAssignableFrom<Task<IEnumerable<BrewResource>>>(result);
            //Assert.AreEqual(brewName, result.Result.ElementAt(0).Name);
        }
    }
}