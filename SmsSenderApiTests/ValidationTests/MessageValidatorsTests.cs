using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System;
using Xunit;
using SmsSenderApi.Validators.Implementation;
using SmsSenderApi.Enums;

namespace SmsSenderApiTests.ValidationTests
{
    public class MessageValidatorsTests
    {
        private readonly IValidator<string> _emptyMessageValidator;
        private readonly IValidator<string> _gsmAndTransliteratedCyrillicValidator;
        private readonly IValidator<string> _messageLengthValidator;


        public MessageValidatorsTests()
        {
            _emptyMessageValidator = new EmptyMessageValidator();
            _gsmAndTransliteratedCyrillicValidator = new GsmAndTransliteratedCyrillicValidator();
            _messageLengthValidator = new MessageLengthValidator();
        }

        [Fact]
        public void EmptyMessageValidator_NotEmptyMessagesEntered_TrueExpected()
        {
            //Arrange
            var firstMessage = "     Hello    ";
            var secondMessage = "https://google.com/invite";
            var thirdMessage = "1233";

            //Act
            var firstActual = _emptyMessageValidator.Validate(firstMessage);
            var secondActual = _emptyMessageValidator.Validate(secondMessage);
            var thirdActual = _emptyMessageValidator.Validate(thirdMessage);

            //Assert
            Assert.True(firstActual);
            Assert.True(secondActual);
            Assert.True(thirdActual);
        }

        [Fact]
        public void EmptyMessageValidator_EmptyMessagesEntered_ExceptionExpected()
        {
            //Arrange
            var firstMessage = "            ";
            var secondMessage = "";

            var exceptionError = "MESSAGE_EMPTY";
            var exceptionMessage = "Invite message is missing.";
            var exceptionErrorCode = ErrorCode.EmptyMessage;

            //Act
            Action firstAct = () => _emptyMessageValidator.Validate(firstMessage);
            Action secondAct = () => _emptyMessageValidator.Validate(secondMessage);

            //Assert
            ValidationException firstExc = Assert.Throws<ValidationException>(firstAct);
            ValidationException secondtExc = Assert.Throws<ValidationException>(secondAct);

            Assert.Equal(exceptionError, firstExc.Error);
            Assert.Equal(exceptionMessage, firstExc.Message);
            Assert.Equal(exceptionErrorCode, firstExc.ErrorCode);

            Assert.Equal(exceptionError, secondtExc.Error);
            Assert.Equal(exceptionMessage, secondtExc.Message);
            Assert.Equal(exceptionErrorCode, secondtExc.ErrorCode);

        }

        [Fact]
        public void GsmAndTransliteratedCyrillicValidator_GsmAndTransliteratedCirillicMessagesEntered_TrueExpected()
        {
            //Arrange
            var firstMessage = "     Hello     ";
            var secondMessage = "https://google.com/invite";
            var thirdMessage = "Корабли лавировали + сушка 'sdsØ-ÅΔ 12§ñüàж";
            var fourthMessage = "ds/.s%1ррsssasd/.,,,.,<>]']'@#$@#%^&#^&*$^{}~]|€Æ []{}~";

            //Act
            var firstActual = _gsmAndTransliteratedCyrillicValidator.Validate(firstMessage);
            var secondActual = _gsmAndTransliteratedCyrillicValidator.Validate(secondMessage);
            var thirdActual = _gsmAndTransliteratedCyrillicValidator.Validate(thirdMessage);
            var fourthActual = _gsmAndTransliteratedCyrillicValidator.Validate(fourthMessage);

            //Assert
            Assert.True(firstActual);
            Assert.True(secondActual);
            Assert.True(thirdActual);
            Assert.True(fourthActual);
        }

        [Fact]
        public void GsmAndTransliteratedCyrillicValidator_NotGsmAndNotTransliteratedCirillicMessagesEntered_ExceptionExpected()
        {
            //Arrange
            var firstMessage = "лопастЬ"; // Ь и Ъ не переводятся транслитерацией
            var secondMessage = "✴▶◀";

            var exceptionError = "MESSAGE_INVALID";
            var exceptionMessage = "Invitemessage should contain only characters in 7 - bit GSM encoding or Cyrillic letters as well.";
            var exceptionErrorCode = ErrorCode.NotGsmAndTransliteratedCyrillic;

            //Act
            Action firstAct = () => _gsmAndTransliteratedCyrillicValidator.Validate(firstMessage);
            Action secondAct = () => _gsmAndTransliteratedCyrillicValidator.Validate(secondMessage);

            //Assert
            ValidationException firstExc = Assert.Throws<ValidationException>(firstAct);
            ValidationException secondtExc = Assert.Throws<ValidationException>(secondAct);

            Assert.Equal(exceptionError, firstExc.Error);
            Assert.Equal(exceptionMessage, firstExc.Message);
            Assert.Equal(exceptionErrorCode, firstExc.ErrorCode);

            Assert.Equal(exceptionError, secondtExc.Error);
            Assert.Equal(exceptionMessage, secondtExc.Message);
            Assert.Equal(exceptionErrorCode, secondtExc.ErrorCode);
        }

        [Fact]
        public void MessageLengthValidator_RequiredLengthMessagesEntered_TrueExpected()
        {
            //Arrange
            var firstMessage = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello" +
                "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello"; //Length = 160

            //Length = 128
            var secondMessage = "https://google.com/inviteinviteinviteinviteinviteinviteinviteinviteinviteinviteinviteinviteinviteinviteinviteenviteinviteinvitee";

            var thirdMessage = "Корабли лавировали + сушка 'sdsØ-ÅΔ 12§ñüàж";

            //Act
            var firstActual = _messageLengthValidator.Validate(firstMessage);
            var secondActual = _messageLengthValidator.Validate(secondMessage);
            var thirdActual = _messageLengthValidator.Validate(thirdMessage);

            //Assert
            Assert.True(firstActual);
            Assert.True(secondActual);
            Assert.True(thirdActual);
        }

        [Fact]
        public void MessageLengthValidator_IncorrectLengthMessagesEntered_ExceptionExpected()
        {
            //Arrange
            var firstMessage = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello" +
                "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloN"; //Length = 161

            var secondMessage = "Корабли лавировали + сушка 'sdsØ-ÅΔ 12§ñüàжsfкукууКорабли лавировали + сушка 'sdsØ-ÅΔ 12§ñüàжsfкукуу" +
                "сушка 'sdsØ-ÅΔ 12§ñüàжsfкукуу"; //Length = 129

            var exceptionError = "MESSAGE_INVALID";
            var exceptionMessage = "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.";
            var exceptionErrorCode = ErrorCode.TooLongMessage;

            //Act
            Action firstAct = () => _messageLengthValidator.Validate(firstMessage);
            Action secondAct = () => _messageLengthValidator.Validate(secondMessage);

            //Assert
            ValidationException firstExc = Assert.Throws<ValidationException>(firstAct);
            ValidationException secondtExc = Assert.Throws<ValidationException>(secondAct);

            Assert.Equal(exceptionError, firstExc.Error);
            Assert.Equal(exceptionMessage, firstExc.Message);
            Assert.Equal(exceptionErrorCode, firstExc.ErrorCode);

            Assert.Equal(exceptionError, secondtExc.Error);
            Assert.Equal(exceptionMessage, secondtExc.Message);
            Assert.Equal(exceptionErrorCode, secondtExc.ErrorCode);
        }
    }
}
