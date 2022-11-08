using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("PhoneNumbers","EmptyPhoneNumbers")]
    public class EmptyPhoneNumbersValidator : IValidator<List<string>>
    {
        /// <summary>
        /// Проверка на пустой список номеров телефонов.
        /// </summary>
        /// <param name="phoneNumbers">Список номеров телефонов</param>
        /// <exception cref="ValidationException">Список номеров телефонов пустой</exception>
        public bool Validate(List<string> phoneNumbers) =>
            phoneNumbers.Count > 0 ?
                true :
                throw new ValidationException(ErrorCode.EmptyPhoneNumbers, "PHONE_NUMBERS_EMPTY", "Phone numbers are missing.");
       
    }
}
