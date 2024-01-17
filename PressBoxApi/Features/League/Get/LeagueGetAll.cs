using MediatR;
using Microsoft.AspNetCore.Mvc;
using PressBoxApi.Common;
using PressBoxApi.Common.Services;

namespace PressBoxApi.Features.League.Get;

public partial class LeaguesController : ApiControllerBase
{
    [HttpGet]
    public async Task<LeagueGetAll.Response> GetAllLeagues()
    {
        return await Mediator.Send(new LeagueGetAll.Request());
    }
}

public static class LeagueGetAll
{
    public record Request : IRequest<Response>;

    public record Response(IEnumerable<League> Leagues);

    internal sealed class GetAllLeaguesHandler : IRequestHandler<Request, Response>
    {
        private readonly IDataService _dataService;

        public GetAllLeaguesHandler(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var data = await _dataService.GetAllLeaguesAsync();

            return new Response(data ?? new List<League>());
        }
    }
}