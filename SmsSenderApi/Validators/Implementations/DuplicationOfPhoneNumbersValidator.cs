using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("PhoneNumbers", "DuplicationOfPhoneNumbers")]
    public class DuplicationOfPhoneNumbersValidator : IValidator<List<string>>
    {
        /// <summary>
        /// Проверка на совпадение номеров телефонов в списке номеров.
        /// </summary>
        /// <param name="phoneNumbers">Список номеров телефонов</param>
        /// <exception cref="ValidationException">Номера телефонов повторяются</exception>
        public bool Validate(List<string> phoneNumbers) =>
            (phoneNumbers.Count - phoneNumbers.ToHashSet().Count > 0) ?
                throw new ValidationException(ErrorCode.DuplicatedPhoneNumbers, "PHONE_NUMBERS_INVALID",
                    "Duplicate numbers detected.")
                : true;
    }
}
