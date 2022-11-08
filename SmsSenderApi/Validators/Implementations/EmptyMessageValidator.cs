using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("Message", "EmptyMessage")]
    public class EmptyMessageValidator : IValidator<string>
    {
        /// <summary>
        /// Проверка на пустое сообщение.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <exception cref="ValidationException">Сообщение пустое</exception>
        public bool Validate(string message) =>
            string.IsNullOrEmpty(message.Trim()) ?
                throw new ValidationException(ErrorCode.EmptyMessage, "MESSAGE_EMPTY",
                    "Invite message is missing.") :
                true;
    }
}
