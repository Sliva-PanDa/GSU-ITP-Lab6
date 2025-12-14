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
    public class ProjectsControllerTests
    {
        private Mock<IProjectRepository> _mockRepo;
        private ProjectsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IProjectRepository>();
            _controller = new ProjectsController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_CallsGetProjectsAsQueryable_AndThrowsException()
        {
            var projects = new List<Project>().AsQueryable();
            _mockRepo.Setup(repo => repo.GetProjectsAsQueryable()).Returns(projects);
            await Assert.ThrowsExceptionAsync<System.InvalidOperationException>(async () => await _controller.Index(null, 1));
            _mockRepo.Verify(r => r.GetProjectsAsQueryable(), Times.Once);
        }

        [TestMethod]
        public async Task Create_POST_RedirectsToIndex_WhenModelIsValid()
        {
            var newProject = new Project { Name = "Новый Проект", LeaderId = 1 };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);
            var result = await _controller.Create(newProject);
            _mockRepo.Verify(r => r.AddAsync(newProject), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFoundResult_WhenProjectNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project)null);
            var result = await _controller.Details(99);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_GET_ReturnsNotFound_WhenProjectNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project)null);
            var result = await _controller.Edit(99);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_POST_ReturnsView_WhenModelIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Edit(1, new Project { ProjectId = 1 });
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}