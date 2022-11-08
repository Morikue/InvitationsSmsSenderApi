using System;
using System.Collections.Generic;
using Xunit;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Implementation;
using SmsSenderApi.Validators.Interfaces;
using SmsSenderApi.Enums;

namespace SmsSenderApiTests.ValidationTests
{
    public class PhoneNumbersValidatorsTests
    {
        private readonly IValidator<List<string>> _duplicationOfPhoneNumbersValidator;
        private readonly IValidator<List<string>> _internationalFormatNumberValidator;
        private readonly IValidator<List<string>> _phoneNumbersAmountValidator;

        public PhoneNumbersValidatorsTests()
        {
            _duplicationOfPhoneNumbersValidator = new DuplicationOfPhoneNumbersValidator();
            _internationalFormatNumberValidator = new InternationalFormatNumberValidator();
            _phoneNumbersAmountValidator = new PhoneNumbersAmountValidator();
        }

        [Fact]
        public void InternationalFormatNumberValidator_CorrectNumbersEntered_TrueExpected()
        {
            //Arrange
            var phoneNumbersToTest = new List<string>
            {
                "71111111111",
                "79119999999",
                "78110730263",
                "79001234567",
                "72001234568"
            };

            //Act
            var actual = _internationalFormatNumberValidator.Validate(phoneNumbersToTest);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void InternationalFormatNumberValidator_IncorrectNumbersEntered_ExceptionExpected()
        {
            //Arrange
            var phoneNumbersToTest = new List<string>
            {
                "71111111111",
                "28110730263",
                "79001234567",
                "72001234568"
            };

            var exceptionError = "PHONE_NUMBERS_INVALID";
            var exceptionMessage = "One or several phone numbers do not match with international format.";
            var exceptionErrorCode = ErrorCode.NotInternationalFormatPhoneNumber;

            //Act
            Action act = () => _internationalFormatNumberValidator.Validate(phoneNumbersToTest);

            //Assert
            ValidationException exc = Assert.Throws<ValidationException>(act);
            Assert.Equal(exceptionError, exc.Error);
            Assert.Equal(exceptionMessage, exc.Message);
            Assert.Equal(exceptionErrorCode, exc.ErrorCode);

        }

        [Fact]
        public void PhoneNumbersAmountValidator_CorrectNumbersAmountEntered_TrueExpected()
        {
            //Arrange
            var phoneNumbersToTest = new List<string>
            {
                "71111111111",
                "79119999999",
                "78110730263",
                "79001234567",
                "72001234568"
            };

            //Act
            var actual = _phoneNumbersAmountValidator.Validate(phoneNumbersToTest);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void PhoneNumbersAmountValidator_IncorrectNumbersAmountEntered_ExceptionExpected()
        {
            //Arrange
            var phoneNumbersToTest = new List<string> // 17 numbers
            {
                "71111111111",
                "78110730263",
                "79001234567",
                "72001234568",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
                "79001234567",
            };

            var exceptionError = "PHONE_NUMBERS_INVALID";
            var exceptionMessage = "Too much phone numbers, should be less or equal to 16 per request.";
            var exceptionErrorCode = ErrorCode.TooMuchPhoneNumbers;

            //Act
            Action act = () => _phoneNumbersAmountValidator.Validate(phoneNumbersToTest);

            //Assert
            ValidationException exc = Assert.Throws<ValidationException>(act);
            Assert.Equal(exceptionError, exc.Error);
            Assert.Equal(exceptionMessage, exc.Message);
            Assert.Equal(exceptionErrorCode, exc.ErrorCode);

        }

        [Fact]
        public void DuplicationOfPhoneNumbersValidator_NotDuplicatedNumbersEntered_TrueExpected()
        {
            //Arrange
            var phoneNumbersToTest = new List<string>
            {
                "71111111111",
                "79119999999",
                "78110730263",
                "79001234567",
                "72001234568"
            };

            //Act
            var actual = _duplicationOfPhoneNumbersValidator.Validate(phoneNumbersToTest);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void DuplicationOfPhoneNumbersValidator_DuplicatedNumbersEntered_ExceptionExpected()
        {
            //Arrange
            var phoneNumbersToTest = new List<string>
            {
                "71112211111",
                "71111111111",
                "79001234567",
                "79001234567"
            };

            var exceptionError = "PHONE_NUMBERS_INVALID";
            var exceptionMessage = "Duplicate numbers detected.";
            var exceptionErrorCode = ErrorCode.DuplicatedPhoneNumbers;

            //Act
            Action act = () => _duplicationOfPhoneNumbersValidator.Validate(phoneNumbersToTest);

            //Assert
            ValidationException exc = Assert.Throws<ValidationException>(act);
            Assert.Equal(exceptionError, exc.Error);
            Assert.Equal(exceptionMessage, exc.Message);
            Assert.Equal(exceptionErrorCode, exc.ErrorCode);

        }

    }
}
