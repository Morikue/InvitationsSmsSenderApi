using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System.Text.RegularExpressions;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("Message", "MessageLength")]
    public class MessageLengthValidator : IValidator<string>
    {
        private readonly static string latinCharactersPattern = @"^([a-z]|[A-Z]){1,160}$";

        private Regex latinCharactersRegex = new Regex(latinCharactersPattern);

        /// <summary>
        /// Проверка сообщения на длину, меньшую 160 символов для латиницы и 128 для остальных типов символов.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <exception cref="ValidationException">Сообщение больше допустимой длины</exception>
        public bool Validate(string message )
        {
            if (!latinCharactersRegex.IsMatch(message) && message.Length > 128)
                throw new ValidationException(ErrorCode.TooLongMessage, "MESSAGE_INVALID",
                        "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.");

            return true;
        }
    }
}
