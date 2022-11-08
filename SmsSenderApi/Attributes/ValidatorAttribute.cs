using System;

namespace SmsSenderApi.Attributes
{
    public class ValidatorAttribute : Attribute
    {
        public string ValidationObject { get; }
        public string ValidationType { get; }

        public ValidatorAttribute() { }

        public ValidatorAttribute(string validationObject, string validationType) 
        {
            ValidationObject = validationObject;
            ValidationType = validationType;
        }

    }
}
