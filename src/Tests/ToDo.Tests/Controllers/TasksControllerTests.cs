using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Controllers;
using ToDo.Models;
using ToDo.Services;
using ToDo.ViewModels.Tasks;
using Xunit;

namespace ToDo.Tests.Controllers
{
    public class TasksControllerTests
    {
        private const string VALID_ID = "Id";
        private const string INVALID_ID = "Clearly invalid Id";
        private const string EXPECTED_REFERER = "https://example.com";

        private TasksController tasksController = null!;
        private Mock<IToDoService> toDoServiceMock = null!;
        private IEnumerable<TaskModel> expectedTasks;
        private TaskModel expectedTask;
        private Exception exception = null!;
        private object result = null!;

        public TasksControllerTests()
        {
            expectedTask = new TaskModel
            {
                Id = VALID_ID,
                Name = "Name",
                Description = "Description",
                IsCompleted = true,
                DueDate = DateTimeOffset.ParseExact("2000-09-10", "yyyy-MM-dd", null)
            };

            expectedTasks = new[]
            {
                expectedTask
            };
        }

        #region Index

        [Fact]
        public async Task Index_InvokesGetSortedTasksAsync()
        {
            GivenTasksController();
            await WhenIndexInvokedAsync();
            ThenGetSortedTasksAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            GivenTasksController();
            await WhenIndexInvokedAsync();
            ThenResultShouldBeOfType<ViewResult>();
        }

        [Fact]
        public async Task Index_ViewResultModelIsOfIndexType()
        {
            GivenTasksController();
            await WhenIndexInvokedAsync();
            ThenViewResultModelShouldBeOfType<IndexViewModel>();
        }

        [Fact]
        public async Task Index_ViewResultModelContainsCollectionOfTasks()
        {
            GivenTasksController();
            await WhenIndexInvokedAsync();
            ThenViewResultIndexModelShouldContainCollectionOfTasks();
        }

        #endregion

        #region Single

        [Fact]
        public async Task Single_InvalidId_Throws()
        {
            GivenTasksController();
            await WhenSingleInvokedWithInvalidIdAsync();
            ThenExceptionShouldBeThrown();
        }

        [Fact]
        public async Task Single_ValidId_InvokesGetTaskAsync()
        {
            GivenTasksController();
            await WhenSingleInvokedWithValidIdAsync();
            ThenGetTaskAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task Single_ValidId_ReturnsViewResult()
        {
            GivenTasksController();
            await WhenSingleInvokedWithValidIdAsync();
            ThenResultShouldBeOfType<ViewResult>();
        }

        [Fact]
        public async Task Single_ValidId_ViewResultModelIsOfSingleType()
        {
            GivenTasksController();
            await WhenSingleInvokedWithValidIdAsync();
            ThenViewResultModelShouldBeOfType<SingleViewModel>();
        }

        [Fact]
        public async Task Single_ValidId_ViewResultModelContainsTask()
        {
            GivenTasksController();
            await WhenSingleInvokedWithValidIdAsync();
            ThenViewResultSingleModelShouldContainTask();
        }

        #endregion

        #region Complete

        [Fact]
        public async Task Complete_InvalidId_Throws()
        {
            GivenTasksController();
            await WhenCompleteInvokedWithInvalidIdAsync();
            ThenExceptionShouldBeThrown();
        }

        [Fact]
        public async Task Complete_ValidId_InvokesCompleteTaskAsync()
        {
            GivenTasksController();
            await WhenCompleteInvokedWithValidIdAsync();
            ThenCompleteTaskAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task Complete_ValidIdWithReferer_ReturnsRedirectResult()
        {
            GivenTasksControllerWithReferer();
            await WhenCompleteInvokedWithValidIdAsync();
            ThenResultShouldBeOfType<RedirectResult>();
        }

        [Fact]
        public async Task Complete_ValidIdWithReferer_RedirectResultUrlIsReferer()
        {
            GivenTasksControllerWithReferer();
            await WhenCompleteInvokedWithValidIdAsync();
            ThenRedirectResultUrlShouldBeReferer();
        }

        [Fact]
        public async Task Complete_ValidId_ReturnsRedirectToActionResult()
        {
            GivenTasksController();
            await WhenCompleteInvokedWithValidIdAsync();
            ThenResultShouldBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Complete_ValidId_RedirectToActionResultActionIsIndex()
        {
            GivenTasksController();
            await WhenCompleteInvokedWithValidIdAsync();
            ThenRedirectToActionResultActionShouldBeIndex();
        }

        #endregion

        #region Edit

        [Fact]
        public async Task Edit_InvalidId_Throws()
        {
            GivenTasksController();
            await WhenEditInvokedWithInvalidIdAsync();
            ThenExceptionShouldBeThrown();
        }

        [Fact]
        public async Task Edit_ValidId_InvokesGetTaskAsync()
        {
            GivenTasksController();
            await WhenEditInvokedWithValidIdAsync();
            ThenGetTaskAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task Edit_ValidId_ReturnsViewResult()
        {
            GivenTasksController();
            await WhenEditInvokedWithValidIdAsync();
            ThenResultShouldBeOfType<ViewResult>();
        }

        [Fact]
        public async Task Edit_ValidId_ViewResultModelIsOfEditType()
        {
            GivenTasksController();
            await WhenEditInvokedWithValidIdAsync();
            ThenViewResultModelShouldBeOfType<EditViewModel>();
        }

        [Fact]
        public async Task Edit_ValidId_ViewResultModelContainsTask()
        {
            GivenTasksController();
            await WhenEditInvokedWithValidIdAsync();
            ThenViewResultEditModelShouldContainTask();
        }

        #endregion

        #region SubmitEdit

        [Fact]
        public async Task SumbitEdit_InvalidModelState_ReturnsViewResult()
        {
            GivenTasksControllerWithInvalidModelState();
            await WhenSubmitEditInvokedAsync();
            ThenResultShouldBeOfType<ViewResult>();
        }

        [Fact]
        public async Task SubmitEdit_InvalidModelState_ViewResultModelIsOfEditType()
        {
            GivenTasksControllerWithInvalidModelState();
            await WhenSubmitEditInvokedAsync();
            ThenViewResultModelShouldBeOfType<EditViewModel>();
        }

        [Fact]
        public async Task SubmitEdit_InvalidModelState_ViewResultModelContainsTask()
        {
            GivenTasksControllerWithInvalidModelState();
            await WhenSubmitEditInvokedAsync();
            ThenViewResultEditModelShouldContainTask();
        }

        [Fact]
        public async Task SubmitEdit_InvalidModelState_ViewResultViewIsEdit()
        {
            GivenTasksControllerWithInvalidModelState();
            await WhenSubmitEditInvokedAsync();
            ThenViewResultViewShouldBeEdit();
        }

        [Fact]
        public async Task SubmitEdit_ValidModelState_InvokesAddOrUpdateAsync()
        {
            GivenTasksController();
            await WhenSubmitEditInvokedAsync();
            ThenAddOrUpdateAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task SubmitEdit_ValidModelState_ReturnsRedirectToActionResult()
        {
            GivenTasksController();
            await WhenSubmitEditInvokedAsync();
            ThenResultShouldBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task SubmitEdit_ValidModelState_RedirectToActionResultActionIsSingle()
        {
            GivenTasksController();
            await WhenSubmitEditInvokedAsync();
            ThenRedirectToActionResultActionShouldBeSingle();
        }

        [Fact]
        public async Task SubmitEdit_ValidModelState_RedirectToActionResultRouteValuesContainsId()
        {
            GivenTasksController();
            await WhenSubmitEditInvokedAsync();
            ThenRedirectToActionResultRouteValuesShouldContainId();
        }

        #endregion

        #region Create

        [Fact]
        public void Create_ReturnsViewResult()
        {
            GivenTasksController();
            WhenCreateInvoked();
            ThenResultShouldBeOfType<ViewResult>();
        }

        [Fact]
        public void Create_ViewResultModelIsOfEditType()
        {
            GivenTasksController();
            WhenCreateInvoked();
            ThenViewResultModelShouldBeOfType<EditViewModel>();
        }

        [Fact]
        public void Create_ViewResultModelContainsBlankTask()
        {
            GivenTasksController();
            WhenCreateInvoked();
            ThenViewResultEditModelShouldContainBlankTask();
        }

        [Fact]
        public void Create_ViewResultViewIsEdit()
        {
            GivenTasksController();
            WhenCreateInvoked();
            ThenViewResultViewShouldBeEdit();
        }

        #endregion

        #region ToDo

        [Fact]
        public async Task ToDo_InvalidId_Throws()
        {
            GivenTasksController();
            await WhenToDoInvokedWithInvalidIdAsync();
            ThenExceptionShouldBeThrown();
        }

        [Fact]
        public async Task ToDo_ValidId_InvokesToDoTaskAsync()
        {
            GivenTasksController();
            await WhenToDoInvokedWithValidIdAsync();
            ThenToDoTaskAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task ToDo_ValidId_ReturnsRedirectToActionResult()
        {
            GivenTasksController();
            await WhenToDoInvokedWithValidIdAsync();
            ThenResultShouldBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task ToDo_ValidId_RedirectToActionResultActionIsEdit()
        {
            GivenTasksController();
            await WhenToDoInvokedWithValidIdAsync();
            ThenRedirectToActionResultActionShouldBeEdit();
        }

        [Fact]
        public async Task ToDo_ValidId_RedirectToActionResultRouteValuesContainsId()
        {
            GivenTasksController();
            await WhenToDoInvokedWithValidIdAsync();
            ThenRedirectToActionResultRouteValuesShouldContainId();
        }

        #endregion

        #region SubmitDelete

        [Fact]
        public async Task SubmitDelete_InvalidId_Throws()
        {
            GivenTasksController();
            await WhenSubmitDeleteInvokedWithInvalidIdAsync();
            ThenExceptionShouldBeThrown();
        }

        [Fact]
        public async Task SubmitDelete_ValidId_InvokesDeleteTaskAsync()
        {
            GivenTasksController();
            await WhenSubmitDeleteInvokedWithValidIdAsync();
            ThenDeleteTaskAsyncShouldBeInvoked();
        }

        [Fact]
        public async Task SubmitDelete_ValidId_ReturnsRedirectToActionResult()
        {
            GivenTasksController();
            await WhenSubmitDeleteInvokedWithValidIdAsync();
            ThenResultShouldBeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task SubmitDelete_ValidId_RedirectToActionResultActionIsEdit()
        {
            GivenTasksController();
            await WhenSubmitDeleteInvokedWithValidIdAsync();
            ThenRedirectToActionResultActionShouldBeIndex();
        }

        #endregion

        #region States

        private void GivenTasksController()
        {
            toDoServiceMock = new Mock<IToDoService>();

            toDoServiceMock
                .Setup(m => m.GetSortedTasksAsync())
                .Returns(Task.FromResult(expectedTasks));

            toDoServiceMock
                .Setup(m => m.GetTaskAsync(VALID_ID))
                .Returns(Task.FromResult(expectedTask));

            toDoServiceMock
                .Setup(m => m.GetTaskAsync(INVALID_ID))
                .Throws(new Exception());

            toDoServiceMock
                .Setup(m => m.CompleteTaskAsync(INVALID_ID))
                .Throws(new Exception());

            toDoServiceMock
                .Setup(m => m.AddOrUpdateTaskAsync(expectedTask))
                .Returns(Task.FromResult(VALID_ID));

            toDoServiceMock
                .Setup(m => m.ToDoTaskAsync(INVALID_ID))
                .Throws(new Exception());

            toDoServiceMock
                .Setup(m => m.DeleteTaskAsync(INVALID_ID))
                .Throws(new Exception());

            tasksController = new TasksController(toDoServiceMock.Object);
            tasksController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        private void GivenTasksControllerWithReferer()
        {
            GivenTasksController();

            tasksController.ControllerContext.HttpContext.Request.Headers["Referer"] = EXPECTED_REFERER;
        }

        private void GivenTasksControllerWithInvalidModelState()
        {
            GivenTasksController();

            tasksController.ControllerContext.ModelState.AddModelError("Error", "Message");
        }

        #endregion

        #region Behaviors

        private async Task WhenIndexInvokedAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Index();
            });
        }

        private async Task WhenSingleInvokedWithInvalidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Single(INVALID_ID);
            });
        }

        private async Task WhenSingleInvokedWithValidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Single(VALID_ID);
            });
        }

        private async Task WhenCompleteInvokedWithInvalidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Complete(INVALID_ID);
            });
        }

        private async Task WhenCompleteInvokedWithValidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Complete(VALID_ID);
            });
        }

        private async Task WhenEditInvokedWithInvalidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Edit(INVALID_ID);
            });
        }

        private async Task WhenEditInvokedWithValidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.Edit(VALID_ID);
            });
        }

        private async Task WhenSubmitEditInvokedAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.SubmitEdit(expectedTask);
            });
        }

        private void WhenCreateInvoked()
        {
            exception = Record.Exception(() =>
            {
                result = tasksController.Create();
            });
        }

        private async Task WhenToDoInvokedWithInvalidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.ToDo(INVALID_ID);
            });
        }

        private async Task WhenToDoInvokedWithValidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.ToDo(VALID_ID);
            });
        }

        private async Task WhenSubmitDeleteInvokedWithInvalidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.SubmitDelete(INVALID_ID);
            });
        }

        private async Task WhenSubmitDeleteInvokedWithValidIdAsync()
        {
            exception = await Record.ExceptionAsync(async () =>
            {
                result = await tasksController.SubmitDelete(VALID_ID);
            });
        }

        #endregion

        #region Expectations

        private void ThenResultShouldBeOfType<T>()
        {
            Assert.IsAssignableFrom<T>(result);
        }

        private void ThenViewResultModelShouldBeOfType<T>()
        {
            var viewResult = (ViewResult)result;
            var model = viewResult.Model;
            Assert.IsAssignableFrom<T>(model);
        }

        private void ThenViewResultIndexModelShouldContainCollectionOfTasks()
        {
            var viewResult = (ViewResult)result;
            var model = (IndexViewModel)viewResult.Model;
            Assert.Equal(expectedTasks, model.Tasks);
        }

        private void ThenViewResultSingleModelShouldContainTask()
        {
            var viewResult = (ViewResult)result;
            var model = (SingleViewModel)viewResult.Model;
            Assert.Equal(expectedTasks.First(), model.Task);
        }

        private void ThenViewResultEditModelShouldContainTask()
        {
            var viewResult = (ViewResult)result;
            var model = (EditViewModel)viewResult.Model;
            Assert.Equal(expectedTasks.First(), model.Task);
        }

        private void ThenViewResultEditModelShouldContainBlankTask()
        {
            var viewResult = (ViewResult)result;
            var model = (EditViewModel)viewResult.Model;
            Assert.Null(model.Task.Id);
        }

        private void ThenViewResultViewShouldBeEdit()
        {
            var viewResult = (ViewResult)result;
            var viewName = viewResult.ViewName;
            Assert.Equal(nameof(TasksController.Edit), viewName);
        }

        private void ThenRedirectResultUrlShouldBeReferer()
        {
            var redirectResult = (RedirectResult)result;
            var url = redirectResult.Url;
            Assert.Equal(EXPECTED_REFERER, url);
        }

        private void ThenRedirectToActionResultActionShouldBeIndex()
        {
            var redirectToActionResult = (RedirectToActionResult)result;
            var actionName = redirectToActionResult.ActionName;
            Assert.Equal(nameof(TasksController.Index), actionName);
        }

        private void ThenRedirectToActionResultActionShouldBeSingle()
        {
            var redirectToActionResult = (RedirectToActionResult)result;
            var actionName = redirectToActionResult.ActionName;
            Assert.Equal(nameof(TasksController.Single), actionName);
        }

        private void ThenRedirectToActionResultActionShouldBeEdit()
        {
            var redirectToActionResult = (RedirectToActionResult)result;
            var actionName = redirectToActionResult.ActionName;
            Assert.Equal(nameof(TasksController.Edit), actionName);
        }

        private void ThenRedirectToActionResultRouteValuesShouldContainId()
        {
            var redirectToActionResult = (RedirectToActionResult)result;
            var routeValues = (IDictionary<string, object>) redirectToActionResult.RouteValues;
            Assert.Contains("id", routeValues);
            Assert.Equal(routeValues["id"], VALID_ID);
        }

        private void ThenGetSortedTasksAsyncShouldBeInvoked()
        {
            toDoServiceMock.Verify(m => m.GetSortedTasksAsync());
        }
        private void ThenGetTaskAsyncShouldBeInvoked()
        {
            toDoServiceMock.Verify(m => m.GetTaskAsync(It.IsAny<string?>()));
        }

        private void ThenCompleteTaskAsyncShouldBeInvoked()
        {
            toDoServiceMock.Verify(m => m.CompleteTaskAsync(It.IsAny<string?>()));
        }

        private void ThenAddOrUpdateAsyncShouldBeInvoked()
        {
            toDoServiceMock.Verify(m => m.AddOrUpdateTaskAsync(It.IsAny<TaskModel>()));
        }

        private void ThenToDoTaskAsyncShouldBeInvoked()
        {
            toDoServiceMock.Verify(m => m.ToDoTaskAsync(It.IsAny<string?>()));
        }

        private void ThenDeleteTaskAsyncShouldBeInvoked()
        {
            toDoServiceMock.Verify(m => m.DeleteTaskAsync(It.IsAny<string?>()));
        }

        private void ThenExceptionShouldBeThrown()
        {
            Assert.NotNull(exception);
            Assert.IsAssignableFrom<Exception>(exception);
        }

        #endregion
    }
}
