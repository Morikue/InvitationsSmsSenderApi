using SmsSenderApi.Attributes;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("PhoneNumbers","AllPhoneNumberValidations")]
    public class PhoneNumbersAllValidator : IAllValidator<List<string>>
    {
        private readonly IEnumerable<IValidator<List<string>>> _validatorsCollection;

        public PhoneNumbersAllValidator(IEnumerable<IValidator<List<string>>> validatorsCollection)
        {
            _validatorsCollection = validatorsCollection.Where(v => (v.GetType()
                    .GetCustomAttributes(typeof(ValidatorAttribute), false).FirstOrDefault() as ValidatorAttribute)
                    .ValidationObject == "PhoneNumbers"); // Получаем только валлидаторы номеров телефона.
        }

        /// <summary>
        /// Проверяет список номеров телефонов по всем параметрам валидации. 
        /// </summary>
        /// <param name="phoneNumbers">Список номеров телефонов</param>
        public bool ValidateAll(List<string> phoneNumbers)
        {
            foreach (var validator in _validatorsCollection)
            {
                validator.Validate(phoneNumbers);
            }

            return true;
        }

    }
}
