using PressBoxApi.Features.Brand;

namespace PressBoxApi.Features.League;

public record Team(string Id, string Name);

public record TeamWithBranding(string Id, string Name, BrandDetail Brand) : Team(Id, Name);

public record LeagueDetail(string Id, DateTime Date, int GameWeek, Team HomeTeam, Team AwayTeam, string Venue);

public record LeagueDetailWithBranding(
    string Id,
    DateTime Date,
    int GameWeek,
    TeamWithBranding HomeTeam,
    TeamWithBranding AwayTeam,
    string Venue);