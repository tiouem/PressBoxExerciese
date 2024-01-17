using MediatR;
using Microsoft.AspNetCore.Mvc;
using PressBoxApi.Common.Services;

namespace PressBoxApi.Features.League.Get;

public partial class LeaguesController
{
    [HttpGet("{id:int}")]
    public async Task<LeagueDetailsGetById.Response> GetByIdLeagues(int id)
    {
        return await Mediator.Send(new LeagueDetailsGetById.Request(id));
    }
}

public static class LeagueDetailsGetById
{
    public record Request(int Id) : IRequest<Response>;

    public record Response(IEnumerable<LeagueDetail> Leagues);

    internal sealed class GetByIdLeaguesHandler : IRequestHandler<Request, Response>
    {
        private readonly IDataService _dataService;

        public GetByIdLeaguesHandler(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var data = await _dataService.GetLeagueDetailsById(request.Id);

            return new Response(data ?? new List<LeagueDetail>());
        }
    }
}