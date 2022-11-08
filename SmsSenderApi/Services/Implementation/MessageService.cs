using Microsoft.AspNetCore.Mvc;
using SmsSenderApi.Domain;
using SmsSenderApi.Domain.Models;
using SmsSenderApi.Dtos;
using SmsSenderApi.Repositories.Interfaces;
using SmsSenderApi.Services.Interfaces;
using System.Threading.Tasks;
using SmsSenderApi.Exceptions;
using System;
using System.Collections.Generic;
using SmsSenderApi.Validators;
using System.Linq;
using SmsSenderApi.Helpers;
using SmsSenderApi.Validators.Interfaces;
using SmsSenderApi.Attributes;
using SmsSenderApi.Enums;

namespace SmsSenderApi.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAllValidator<string> _messageValidator;
        private readonly IAllValidator<List<string>> _phoneNumberValidator;

        public MessageService(IMessageRepository messageRepository, IAllValidator<string> messageValidator, IAllValidator<List<string>> phoneNumberValidator)
        {
            _messageRepository = messageRepository;
            _messageValidator = messageValidator;
            _phoneNumberValidator = phoneNumberValidator;
        }

        /// <summary>
        /// Отправляет Sms сообщения с приглашениями, а также сохраняет сообщение в базе данных.
        /// </summary>
        /// <param name="invitationsSendDto">Сообщение с коллекцией номеров телефонов</param>
        public async Task<bool> SendInvitesAsync(InvitationsSendDto invitationsSendDto, int apiId)
        {
            var inviteMessage = invitationsSendDto.Message;
            var phoneNumbers = invitationsSendDto.RecipientsPhoneNumbers;

            ValidateMessageAndPhoneNumbers(inviteMessage, phoneNumbers);

            var cleanedPhoneNumbers = await GetNonDuplicatedPhoneNumbersAsync(phoneNumbers);

            await NumberOfInvitesTodayCheckAsync(cleanedPhoneNumbers, apiId); 

            // Здесь должна происходить отправка Sms сообщения по номерам телефона.
            // await SendSmsMessages(inviteMessage, cleanedPhoneNumbers);

            await AddMessagesToDbAsync(inviteMessage, cleanedPhoneNumbers, apiId);   
            
            return true;
        }

        /// <summary>
        /// Возвращает список номеров телефонов, которые не содержатся в базе данных.
        /// </summary>
        /// <param name="phoneNumbers">Коллекция номеров телефонов для проверки</param>
        /// <returns>Коллекция номеров, не встречающихся в базе данных</returns>
        private async Task<List<string>> GetNonDuplicatedPhoneNumbersAsync(List<string> phoneNumbers)
        {
            var usedPhoneNumbers = await _messageRepository.GetPhoneNumbersAsync();

            var validPhoneNumbers = new List<string>(phoneNumbers);

            foreach(var number in phoneNumbers)
            {
                if (usedPhoneNumbers.Contains(number))
                {
                    validPhoneNumbers.Remove(number);
                }
            }

            return validPhoneNumbers;
            
        }

        /// <summary>
        /// Добавляет сообщения в базу данных.
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="phoneNumbers">Список номеров телефонов получателей</param>
        private async Task AddMessagesToDbAsync(string message, List<string> phoneNumbers, int apiId)
        {
            foreach(var number in phoneNumbers)
            {
                _messageRepository.AddMessage(new Message
                {
                    Text = message,
                    RecipientPhoneNumber = number,
                    ApiId = apiId
                });
            }

            await _messageRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Проверяет сообщение и коллекцию номеров телефонов по всем параметрам валидации.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="phoneNumbers">Коллекция номеров телефонов</param>
        private bool ValidateMessageAndPhoneNumbers(string message, List<string> phoneNumbers)
        { 
            _phoneNumberValidator.ValidateAll(phoneNumbers); // Валидатор для всех типов валидаций по номеру телефона

            _messageValidator.ValidateAll(message); // Валидатор для всех типов валидаций по сообщению

            return true;
        }

        /// <summary>
        /// Проверяет, не превысит ли количество Смс сообщений в рамках конкретного запроса максимальное количество сообщений, доступных за один день для конкретного ApiId.
        /// </summary>
        /// <param name="phoneNumbers">Коллекция номеров телефонов</param>
        /// <param name="apiId">ApiId</param>
        /// <exception cref="ValidationException">В случае отправки всех сообщений будет превышен дневной лимит для ApiId</exception>
        private async Task<bool> NumberOfInvitesTodayCheckAsync(List<string> phoneNumbers, int apiId) =>
            await _messageRepository.GetTodayMessagesAmountAsync(apiId) + phoneNumbers.Count > 128 ?
                throw new ValidationException(ErrorCode.TooMuchMessagesToday, "PHONE_NUMBERS_INVALID",
                    "Too much phone numbers, should be less or equal to 128 per day.") :
                true;

    }
}
