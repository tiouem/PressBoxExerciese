using System.Collections.Immutable;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PressBoxApi.Common.Services;
using PressBoxApi.Features.Brand;

namespace PressBoxApi.Features.League.Get;

public partial class LeaguesController
{
    [HttpGet("/LeagueDetailsWithBranding/")]
    public async Task<LeagueDetailsWithBranding.Response> GetByIdLeagueDetailsWithBranding([FromQuery] int leagueId,
        [FromQuery] int brandId)
    {
        return await Mediator.Send(new LeagueDetailsWithBranding.Request(leagueId, brandId));
    }
}

public static class LeagueDetailsWithBranding
{
    public record Request(int LeagueId, int BrandId) : IRequest<Response>;

    public record Response(IEnumerable<LeagueDetailWithBranding> LeagueDetailsWithBranding);

    internal sealed class LeagueDetailsWithBrandingHandler : IRequestHandler<Request, Response>
    {
        private readonly IDataService _dataService;

        public LeagueDetailsWithBrandingHandler(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var matchesData = await _dataService.GetLeagueDetailsById(request.LeagueId);
            var brandDetailsData = await _dataService.GetBrandDetailsById(request.BrandId);

            var brandDetails = brandDetailsData!.ToImmutableDictionary(x => x.TeamId);

            var result = GetBrandedMatches(matchesData!, brandDetails);
            return new Response(result);
        }

        private IEnumerable<LeagueDetailWithBranding> GetBrandedMatches(IEnumerable<LeagueDetail>? matchesData,
            ImmutableDictionary<string, BrandDetail> brandDetailsData)
        {
            foreach (var match in matchesData)
            {
                yield return new LeagueDetailWithBranding(match.Id,
                    match.Date,
                    match.GameWeek,
                    AddBrandToTeam(match.HomeTeam, brandDetailsData[match.HomeTeam.Id]),
                    AddBrandToTeam(match.AwayTeam, brandDetailsData[match.AwayTeam.Id]),
                    match.Venue);
                continue;

                TeamWithBranding AddBrandToTeam(Team team, BrandDetail brandDetail)
                {
                    return new TeamWithBranding(team.Id, team.Name,
                        new BrandDetail(brandDetail.TeamId, brandDetail.Name, brandDetail.PrimaryColor,
                            brandDetail.Abbreviation));
                }
            }
        }
    }
}