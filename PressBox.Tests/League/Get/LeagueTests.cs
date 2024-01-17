using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json.Linq;
using PressBoxApi.Common.Services;
using PressBoxApi.Features.League.Get;

namespace PressBox.Tests.League;

[TestFixture]
public class LeagueTests
{
    // INTEGRATION
    [Test]
    public async Task GetAllLeagues_ReturnsCorrectData_Integration()
    {
        var jsonString = await File.ReadAllTextAsync(@"./League/Get/Json/Data.json");

        await using var appFactory = new CustomWebApplicationFactory(services =>
        {
            services.Replace(ServiceDescriptor.Scoped(_ =>
            {
                var dataServiceMock = new Mock<IDataService>();
                dataServiceMock.Setup(x => x.GetAllLeaguesAsync())
                    .ReturnsAsync(JsonSerializer.Deserialize<List<PressBoxApi.Features.League.League>>(jsonString, new
                        JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }));
                return dataServiceMock.Object;
            }));
        });

        var client = appFactory.CreateClient();

        var response = await client.GetAsync("api/Leagues");
        var s = response.Content.ReadAsStringAsync().Result;
        var responseContent = JsonSerializer.Deserialize<LeagueGetAll.Response>( await response.Content.ReadAsStringAsync(), new
            JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        
        responseContent!.Leagues.Should().HaveCount(3);
    }

    // UNIT
    [Test]
    public async Task GetAllLeagues_ReturnsCorrectData_Unit()
    {
        var request = new Mock<LeagueGetAll.Request>();
        var dataServiceMock = new Mock<IDataService>();
        dataServiceMock.Setup(x => x.GetAllLeaguesAsync()).ReturnsAsync(new List<PressBoxApi.Features.League.League>()
            { new("1", "asd") });

        var handler = new LeagueGetAll.GetAllLeaguesHandler(dataServiceMock.Object);

        var res = await handler.Handle(request.Object, It.IsAny<CancellationToken>());

        dataServiceMock.Verify(x => x.GetAllLeaguesAsync(), Times.Once);
    }
}