using Microsoft.AspNetCore.Mvc;
using Moq;
using SmsSenderApi.Controllers;
using SmsSenderApi.Dtos;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SmsSenderApiTests.ControllersTests
{
    public class InviteControllerTests
    {
        private readonly InviteController _controller;
        private readonly Mock<IMessageService> _serviceMock = new Mock<IMessageService>();

        public InviteControllerTests()
        {
            _controller = new InviteController(_serviceMock.Object);
        }

        [Fact]
        public async Task SendInvitationsAsync_RightRequestEntered_OkResultExpected()
        {
            //Arrange
            var expectedActionResult = new JsonResult("Приглашения успешно отправлены!") 
                { StatusCode = 200 };

            var invitationsToSend = new InvitationsSendDto(new List<string>(), "message");

            _serviceMock.Setup(s => s.SendInvitesAsync(It.IsAny<InvitationsSendDto>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var actual = await _controller.SendInvitationsAsync(invitationsToSend);
            
            //Assert 
            Assert.Equal(expectedActionResult.StatusCode, (actual as JsonResult).StatusCode);
            Assert.Equal(expectedActionResult.Value, (actual as JsonResult).Value);
        }

        [Fact]
        public async Task SendInvitationsAsync_ValidationExceptionEntered_BadRequestExpected()
        {
            //Arrange
            var expectedActionResult = new JsonResult(new Dictionary<string, string>()
            {
                {"error", "PHONE_NUMBERS_INVALID" },
                {"message", "One or several phone numbers do not match with international format."}
            })
            { StatusCode = 400 };

            var invitationsToTest = new InvitationsSendDto(new List<string>(), "message");

            _serviceMock.Setup(s => s.SendInvitesAsync(It.IsAny<InvitationsSendDto>(), It.IsAny<int>()))
                .ThrowsAsync(new ValidationException(ErrorCode.NotInternationalFormatPhoneNumber, "PHONE_NUMBERS_INVALID",
                        "One or several phone numbers do not match with international format."));

            //Act
            var actual = await _controller.SendInvitationsAsync(invitationsToTest);

            //Assert 
            Assert.Equal(expectedActionResult.StatusCode, (actual as JsonResult).StatusCode);
            Assert.Equal(expectedActionResult.Value, (actual as JsonResult).Value);
        }

    }
}
