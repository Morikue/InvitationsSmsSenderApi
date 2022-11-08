using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Validators.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SmsSenderApi.Validators.Implementation
{
    [ValidatorAttribute("Message", "GsmAndTransliteratedCyrillic")]
    public class GsmAndTransliteratedCyrillicValidator : IValidator<string>
    {
        private readonly static string gsmCharacters = "@£$¥èéùìòÇØøÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ ^}{\\]~[!\"#¤%&'|€()*+,-./0123456789:;<=>?" +
                "¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà"; // GSM 7-bit

        // Транслитерационная кириллица
        private readonly static string transliteratedCyrillicCharacters = "AБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЫЭЮЯабвгдеёжзийклмнопрстуфхцчшщыэюя"; 

        /// <summary>
        /// Проверка сообщения на соответствие кодировке GSM 7-bit или транслитерационной кириллице.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <exception cref="ValidationException">Сообщение содержит невалидные символы</exception>
        public bool Validate(string message)
        {
            string allowedCharacters = gsmCharacters + transliteratedCyrillicCharacters;

            if (!(Regex.Match(message, @$"[^{allowedCharacters}]").Captures.Count == 0))
                throw new ValidationException(ErrorCode.NotGsmAndTransliteratedCyrillic, "MESSAGE_INVALID",
                    "Invitemessage should contain only characters in 7 - bit GSM encoding or Cyrillic letters as well.");

            return true;
        }
    }
}
