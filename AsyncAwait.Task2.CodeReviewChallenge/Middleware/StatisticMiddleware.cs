using System;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StatisticMiddleware> _logger;
    private readonly IStatisticService _statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService, ILogger<StatisticMiddleware> logger)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;

        await _statisticService.RegisterVisitAsync(path);
        var task = _statisticService.GetVisitsCountAsync(path);
        _logger.LogInformation("GetVisitsCountAsync status: {Status}", task.Status);
        var count = await task;
        context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, count.ToString());

        await _next(context);
    }
}
