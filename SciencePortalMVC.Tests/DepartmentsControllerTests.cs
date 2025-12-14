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
    public class DepartmentsControllerTests
    {
        private Mock<IDepartmentRepository> _mockRepo;
        private DepartmentsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IDepartmentRepository>();
            _controller = new DepartmentsController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_CallsGetDepartmentsAsQueryable_AndThrowsException()
        {
            var departments = new List<Department>().AsQueryable();
            _mockRepo.Setup(r => r.GetDepartmentsAsQueryable()).Returns(departments);

            await Assert.ThrowsExceptionAsync<System.InvalidOperationException>(async () =>
            {
                await _controller.Index(null, 1);
            });

            _mockRepo.Verify(r => r.GetDepartmentsAsQueryable(), Times.Once);
        }

        [TestMethod]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFoundResult_WhenDepartmentNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Department)null);
            var result = await _controller.Details(99);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WithDepartment_WhenFound()
        {
            var department = new Department { DepartmentId = 1, Name = "Test Dept" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);
            var result = await _controller.Details(1);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(department, viewResult.Model);
        }

        [TestMethod]
        public async Task Create_GET_ReturnsViewResult()
        {
            var result = _controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Create_POST_ReturnsView_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.Create(new Department());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Create_POST_CallsAddAsyncAndRedirects_WhenModelStateIsValid()
        {
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Department>())).Returns(Task.CompletedTask);
            var result = await _controller.Create(new Department { Name = "New Dept" });
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Department>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task Edit_GET_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Edit((int?)null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_POST_ReturnsNotFound_WhenIdDoesNotMatch()
        {
            var result = await _controller.Edit(1, new Department { DepartmentId = 2 });
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_POST_CallsUpdateAsyncAndRedirects_WhenModelIsValid()
        {
            var department = new Department { DepartmentId = 1, Name = "Updated Dept" };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Department>())).Returns(Task.CompletedTask);
            var result = await _controller.Edit(1, department);
            _mockRepo.Verify(r => r.UpdateAsync(department), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task Delete_GET_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Delete(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteConfirmed_CallsDeleteAsyncAndRedirects()
        {
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var result = await _controller.DeleteConfirmed(1);
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}