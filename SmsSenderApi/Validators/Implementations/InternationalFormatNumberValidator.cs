using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("PhoneNumbers","InternationalFormatNumber")]
    public class InternationalFormatNumberValidator: IValidator<List<string>>
    {
        private static string phoneNumberPattern = @"^(7)+(([0-9]){10})$";

        private Regex phoneNumberRegex = new Regex(phoneNumberPattern);

        /// <summary>
        /// Проверяет номера телефонов на международный формат телефонных номеров.
        /// </summary>
        /// <param name="phoneNumbers">Список номеров телефонов</param>
        /// <exception cref="ValidationException">Один или несколько номеров не соответствуют международному формату</exception>
        public bool Validate(List<string> phoneNumbers)
        {
            foreach (var number in phoneNumbers)
            {
                if (!phoneNumberRegex.IsMatch(number))
                    throw new ValidationException(ErrorCode.NotInternationalFormatPhoneNumber, "PHONE_NUMBERS_INVALID",
                        "One or several phone numbers do not match with international format.");
            }

            return true;
        }
    }
}
