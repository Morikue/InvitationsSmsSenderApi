using SmsSenderApi.Attributes;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("Message", "AllMessageValidations")]
    public class MessageAllValidator : IAllValidator<string>
    {
        private readonly IEnumerable<IValidator<string>> _validatorsCollection;
        public MessageAllValidator(IEnumerable<IValidator<string>> validatorsCollection)
        {
            _validatorsCollection = validatorsCollection.Where(v => (v.GetType()
                    .GetCustomAttributes(typeof(ValidatorAttribute), false).FirstOrDefault() as ValidatorAttribute)
                    .ValidationObject == "Message"); // Получаем только валидаторы сообщения.
        }

        /// <summary>
        /// Проверяет сообщение по всем параметрам валидации.
        /// </summary>
        /// <param name="message">Сообщение</param>
        public bool ValidateAll(string message)
        {
            foreach (var validator in _validatorsCollection)
            {
                    validator.Validate(message);
            }

            return true;
        }
    }
}
