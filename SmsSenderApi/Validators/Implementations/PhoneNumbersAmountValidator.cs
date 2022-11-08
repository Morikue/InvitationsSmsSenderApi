using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("PhoneNumbers","PhoneNumbersAmount")]
    public class PhoneNumbersAmountValidator : IValidator<List<string>>
    {
        /// <summary>
        /// Проверка на ограниченное количество номеров телефонов в списке, которое не должно быть больше 16.
        /// </summary>
        /// <param name="phoneNumbers">Список номеров телефонов</param>
        /// <exception cref="ValidationException">Номеров телефонов больше 16</exception>
        public bool Validate(List<string> phoneNumbers) =>
            phoneNumbers.Count > 16 ?
                throw new ValidationException(ErrorCode.TooMuchPhoneNumbers, "PHONE_NUMBERS_INVALID",
                    "Too much phone numbers, should be less or equal to 16 per request.") :
                true;
    }
}
