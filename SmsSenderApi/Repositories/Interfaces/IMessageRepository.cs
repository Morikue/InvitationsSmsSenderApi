using SmsSenderApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmsSenderApi.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessagesAsync();

        Task<int> GetTodayMessagesAmountAsync(int apiId);

        Task<List<string>> GetPhoneNumbersAsync();

        void AddMessage(Message message);

        Task<int> SaveChangesAsync();
    }
}
