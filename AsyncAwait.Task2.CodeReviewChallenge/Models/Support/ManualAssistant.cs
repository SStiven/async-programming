using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support;

public class ManualAssistant : IAssistant
{
    private readonly ISupportService _supportService;
    private readonly ILogger<ManualAssistant> _logger;

    public ManualAssistant(ISupportService supportService, ILogger<ManualAssistant> logger)
    {
        _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> RequestAssistanceAsync(string requestInfo)
    {
        try
        {
            var task = _supportService.RegisterSupportRequestAsync(requestInfo);
            _logger.LogInformation("RegisterSupportRequestAsync status: {Status}", requestInfo);
            await task;

            return await _supportService.GetSupportInfoAsync(requestInfo);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while gettingn support info {RequestInfo}", requestInfo);

            return $"Failed to register assistance request. Please try later. {ex.Message}";
        }
    }
}
