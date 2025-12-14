using Microsoft.AspNetCore.Mvc;
using Moq;
using SciencePortalMVC.Controllers;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using SciencePortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Tests
{
    [TestClass]
    public class ProjectsApiControllerTests
    {
        private Mock<IProjectRepository> _mockProjectRepo;
        private Mock<ITeacherRepository> _mockTeacherRepo;
        private ProjectsApiController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockProjectRepo = new Mock<IProjectRepository>();
            _mockTeacherRepo = new Mock<ITeacherRepository>();
            _controller = new ProjectsApiController(_mockProjectRepo.Object, _mockTeacherRepo.Object);
        }

        [TestMethod]
        public async Task GetProjects_ReturnsOkWithProjectDtos()
        {
            var projects = new List<Project> { new Project { Leader = new Teacher() } };
            _mockProjectRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projects);
            var result = await _controller.GetProjects();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetProject_ReturnsOk_WhenProjectExists()
        {
            var project = new Project { ProjectId = 1, Name = "Test", Leader = new Teacher() };
            _mockProjectRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);
            var result = await _controller.GetProject(1);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            _mockProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project)null);
            var result = await _controller.GetProject(99);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PostProject_ReturnsBadRequest_WhenLeaderDoesNotExist()
        {
            _mockTeacherRepo.Setup(r => r.TeacherExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var newProjectDto = new CreateProjectApiDto { Name = "Новый Проект", LeaderId = 999 };
            var result = await _controller.PostProject(newProjectDto);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task PostProject_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            _mockTeacherRepo.Setup(r => r.TeacherExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _mockProjectRepo.Setup(r => r.AddAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);
            _mockProjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project { Leader = new Teacher() });

            var newProjectDto = new CreateProjectApiDto { Name = "Проект Гамма", LeaderId = 1 };
            var result = await _controller.PostProject(newProjectDto);

            _mockProjectRepo.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task DeleteProject_ReturnsNoContent_WhenProjectExists()
        {
            _mockProjectRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Project());
            _mockProjectRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteProject(1);
            _mockProjectRepo.Verify(r => r.DeleteAsync(1), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PutProject_ReturnsBadRequest_WhenIdMismatch()
        {
            var updateDto = new UpdateProjectApiDto { ProjectId = 2 };
            var result = await _controller.PutProject(1, updateDto);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task PutProject_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            _mockTeacherRepo.Setup(r => r.TeacherExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _mockProjectRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Project());
            _mockProjectRepo.Setup(r => r.UpdateAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);

            var updateDto = new UpdateProjectApiDto { ProjectId = 1, Name = "Обновленный", LeaderId = 2 };
            var result = await _controller.PutProject(1, updateDto);

            _mockProjectRepo.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}