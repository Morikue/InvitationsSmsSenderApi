using Moq;
using System.Threading.Tasks;
using Xunit;
using SmsSenderApi.Services.Interfaces;
using SmsSenderApi.Services.Implementation;
using SmsSenderApi.Repositories.Interfaces;
using SmsSenderApi.Domain.Models;
using System.Collections.Generic;
using SmsSenderApi.Dtos;
using System.Linq;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using SmsSenderApi.Enums;

namespace SmsSenderApiTests.ServicesTests
{
    public class MessageServiceTests
    {
        private readonly IMessageService _service;
        private readonly Mock<IMessageRepository> _repositoryMock = new Mock<IMessageRepository>();
        private readonly Mock<IAllValidator<string>> _messageValidatorMock = new Mock<IAllValidator<string>>();
        private readonly Mock<IAllValidator<List<string>>> _phoneNumbersValidatorsMock = new Mock<IAllValidator<List<string>>>();

        public MessageServiceTests()
        {
            _service = new MessageService(_repositoryMock.Object, _messageValidatorMock.Object, _phoneNumbersValidatorsMock.Object);
        }

        [Fact]
        public async Task SendInvitationsAsync_FiveNumbersWithMessageEntered_SaveThreeMessagesExpected()
        {
            //Arrange            
            var apiIdToTest = 4;

            var dtoInviteToTest = new InvitationsSendDto(

                 new List<string> {
                    "71111111111",
                    "79119999999",
                    "78110730263",
                    "79001234567",
                    "79001234568"
                },
                "Hello"
            );

            var usedPhoneNumbers = new List<string>
            {
                "79119999999",
                "79001234567"
            };

            var addedMessages = new List<Message>();

            var messagesExpected = new List<Message>()
            {
                new Message
                {
                    Text = "Hello",
                    RecipientPhoneNumber = "71111111111",
                    ApiId = 4
                },
                new Message
                {
                    Text = "Hello",
                    RecipientPhoneNumber = "78110730263",
                    ApiId = 4
                },
                new Message
                {
                    Text = "Hello",
                    RecipientPhoneNumber = "79001234568",
                    ApiId = 4
                }

            };

            _repositoryMock.Setup(r => r.GetPhoneNumbersAsync())
                .ReturnsAsync(usedPhoneNumbers);

            _repositoryMock.Setup(r => r.AddMessage(It.IsAny<Message>()))
                .Callback((Message message) => addedMessages.Add(message));

            _messageValidatorMock.Setup(v => v.ValidateAll(It.IsAny<string>()))
                .Returns(true);

            _phoneNumbersValidatorsMock.Setup(v => v.ValidateAll(It.IsAny<List<string>>()))
                .Returns(true);

            //Act

            await _service.SendInvitesAsync(dtoInviteToTest, apiIdToTest);

            //Assert

            Assert.Equal(messagesExpected.Select(m => m.RecipientPhoneNumber), addedMessages.Select(m => m.RecipientPhoneNumber));
            Assert.Equal(messagesExpected.Select(m => m.Text), addedMessages.Select(m => m.Text));
            Assert.Equal(messagesExpected.Select(m => m.ApiId), addedMessages.Select(m => m.ApiId));
        }

        [Fact]
        public async Task SendInvitationsAsync_TooManyMessagesHaveBeenSendTodayEntered_ExceptionExpected()
        {
            //Arrange            
            var apiId = 4;

            var dtoInviteToTest = new InvitationsSendDto(

                 new List<string>
                {
                    "71112211111",
                    "71111111111",
                    "79001234567",
                    "79001234562"
                },
                "Hello"
            );

            var messagesToday = 125;

            var exceptionError = "PHONE_NUMBERS_INVALID";
            var exceptionMessage = "Too much phone numbers, should be less or equal to 128 per day.";
            var exceptionErrorCode = ErrorCode.TooMuchMessagesToday;

            _messageValidatorMock.Setup(v => v.ValidateAll(It.IsAny<string>()))
                .Returns(true);

            _phoneNumbersValidatorsMock.Setup(v => v.ValidateAll(It.IsAny<List<string>>()))
                .Returns(true);

            _repositoryMock.Setup(r => r.GetPhoneNumbersAsync()).ReturnsAsync(new List<string>());

            _repositoryMock.Setup(r => r.GetTodayMessagesAmountAsync(It.IsAny<int>())).ReturnsAsync(messagesToday);

            //Act

            Task act() => _service.SendInvitesAsync(dtoInviteToTest, apiId);


            //Assert

            var exc = await Assert.ThrowsAsync<ValidationException>(act);
            Assert.Equal(exceptionError, exc.Error);
            Assert.Equal(exceptionMessage, exc.Message);
            Assert.Equal(exceptionErrorCode, exc.ErrorCode);

        }


    }
}
