using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmsSenderApi.Dtos
{
    public class InvitationsSendDto
    {

        /// <summary>
        /// Номера телефонов получателей
        /// </summary>
        public List<string> RecipientsPhoneNumbers { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Message { get; set; }

        public InvitationsSendDto(List<string> recipientsPhoneNumbers, string message)
        {
            this.RecipientsPhoneNumbers = recipientsPhoneNumbers;
            this.Message = message;
        }
    }
}
