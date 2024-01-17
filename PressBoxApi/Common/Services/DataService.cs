using Microsoft.Extensions.Options;
using PressBoxApi.Features.Brand;
using PressBoxApi.Features.League;

namespace PressBoxApi.Common.Services;

public sealed class DataService : IDataService
{
    private readonly HttpClient _client;
    private readonly string _token;
 

    public DataService(HttpClient client, IOptions<DataSourceSettings> dataSourceSettings)
    {
        _client = client;
        _token = dataSourceSettings.Value.AccessToken;
        // _token = Environment.GetEnvironmentVariable("AccessToken") ??
        //          throw new InvalidOperationException("Token not found in env variables");
    }

    public async Task<IEnumerable<League>?> GetAllLeaguesAsync()
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<League>>($"leagues.json" + _token);
    }

    public async Task<IEnumerable<LeagueDetail>?> GetLeagueDetailsById(int id)
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<LeagueDetail>>($"leagues/{id}.json" + _token);
    }

    public async Task<IEnumerable<Brand>?> GetAllBrandsAsync()
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<Brand>>($"brands.json" + _token);
    }

    public async Task<IEnumerable<BrandDetail>?> GetBrandDetailsById(int id)
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<BrandDetail>>($"brands/{id}.json" + _token);
    }
}

public interface IDataService
{
    Task<IEnumerable<League>?> GetAllLeaguesAsync();
    Task<IEnumerable<LeagueDetail>?> GetLeagueDetailsById(int id);
    Task<IEnumerable<Brand>?> GetAllBrandsAsync();
    Task<IEnumerable<BrandDetail>?> GetBrandDetailsById(int id);
}