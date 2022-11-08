using Microsoft.AspNetCore.Mvc;
using SmsSenderApi.Domain.Models;
using SmsSenderApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmsSenderApi.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> SendInvitesAsync(InvitationsSendDto invitationsSendDto, int apiId);

    }
}
