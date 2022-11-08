using Microsoft.EntityFrameworkCore;
using SmsSenderApi.Domain;
using SmsSenderApi.Domain.Models;
using SmsSenderApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmsSenderApi.Repositories.EntityFramework
{
    public class EFMessageRepository : IMessageRepository
    {
        private readonly SmsSenderDbContext _context;

        public EFMessageRepository(SmsSenderDbContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        /// <summary>
        /// Возвращает коллекцию номеров телефонов.
        /// </summary>
        /// <returns>Коллекция номеров телефонов</returns>
        public async Task<List<string>> GetPhoneNumbersAsync()
        {
            return await _context.Messages.Select(m => m.RecipientPhoneNumber).ToListAsync();
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Возвращает количество отправленных сообщений за сегодня. 
        /// </summary>
        /// <param name="apiId">ApiId</param>
        /// <returns>Количество сообщений за сегодняшний день</returns>
        public async Task<int> GetTodayMessagesAmountAsync(int apiId)
        {
            return await _context.Messages.Where(m => m.ApiId == apiId)
                .Where(m => m.DateOfCreation >= DateTime.Today)
                .Where(m => m.DateOfCreation < DateTime.Today.AddDays(1))
                .CountAsync();
        }
    }
}
