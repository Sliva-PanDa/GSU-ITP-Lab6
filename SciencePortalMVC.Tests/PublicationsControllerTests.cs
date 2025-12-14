using Microsoft.AspNetCore.Mvc;
using Moq;
using SciencePortalMVC.Controllers;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Tests
{
    [TestClass]
    public class PublicationsControllerTests
    {
        private Mock<IPublicationRepository> _mockRepo;
        private PublicationsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPublicationRepository>();
            _controller = new PublicationsController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_CallsGetPublicationsAsQueryable_AndThrowsException()
        {
            var publications = new List<Publication>().AsQueryable();
            _mockRepo.Setup(repo => repo.GetPublicationsAsQueryable()).Returns(publications);
            await Assert.ThrowsExceptionAsync<System.InvalidOperationException>(async () => await _controller.Index(null, 1));
            _mockRepo.Verify(r => r.GetPublicationsAsQueryable(), Times.Once);
        }

        [TestMethod]
        public async Task Create_POST_RedirectsToIndex_WhenModelIsValid()
        {
            var newPublication = new Publication { Title = "Новая публикация", Year = 2026, Type = "Статья" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Publication>())).Returns(Task.CompletedTask);
            var result = await _controller.Create(newPublication);
            _mockRepo.Verify(r => r.AddAsync(newPublication), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFoundResult_WhenPublicationNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Publication)null);
            var result = await _controller.Details(99);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_POST_ReturnsNotFoundResult_WhenIdMismatch()
        {
            var result = await _controller.Edit(1, new Publication { PublicationId = 2 });
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_GET_ReturnsViewWithPublication_WhenFound()
        {
            var publication = new Publication { PublicationId = 1, Title = "Test" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(publication);
            var result = await _controller.Delete(1);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(publication, viewResult.Model);
        }
    }
}