using System;
using System.Threading.Tasks;
using Warden.Services.WardenChecks.Shared.Dto;

namespace Warden.Services.Pusher.Services
{
    public interface ICheckResultService
    {
        Task PublishCheckResultCreatedAsync(Guid organizationId, Guid wardenId, CheckResultDto checkResult);
    }
}