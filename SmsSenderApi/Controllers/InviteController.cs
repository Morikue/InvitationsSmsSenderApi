using Microsoft.AspNetCore.Mvc;
using SmsSenderApi.Dtos;
using SmsSenderApi.Services.Implementation;
using SmsSenderApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmsSenderApi.Exceptions;
using SmsSenderApi.Helpers;
using System;
using SmsSenderApi.Enums;

namespace SmsSenderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {

        private const int ApiId = 4;

        private readonly IMessageService _messageService;

        public InviteController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        //POST api/invite/send
        [HttpPost("send")]
        public async Task<ActionResult> SendInvitationsAsync(InvitationsSendDto invitationsSendDto)
        {
            try
            {
                await _messageService.SendInvitesAsync(invitationsSendDto, ApiId);

                return new HttpResultBuilder().SetMessage("Приглашения успешно отправлены!").Build();
            }
            catch (ValidationException exc)
            {
                return new HttpResultBuilder().SetErrorCode(exc.ErrorCode).SetError( exc.Error).SetMessage(exc.Message).Build();
            }
            catch (Exception exc)
            {
                return new HttpResultBuilder().SetErrorCode(ErrorCode.IternalErrorStatusCode).SetMessage(exc.Message).Build();
            }
        }
    }
}
