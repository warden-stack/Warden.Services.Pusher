using System;
using Warden.Services.WardenChecks.Shared.Dto;

namespace Warden.Services.Pusher.Services
{
    public interface ISignalRService
    {
        void SendCheckResultSaved(Guid organizationId, Guid wardenId, CheckResultDto checkResult);
    }
}