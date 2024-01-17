using MediatR;
using Microsoft.AspNetCore.Mvc;
using PressBoxApi.Common;
using PressBoxApi.Common.Services;

namespace PressBoxApi.Features.Brand.Get;

public class BrandsController : ApiControllerBase
{
    [HttpGet]
    public async Task<BrandGetAll.Response> GetAllBrands()
    {
        return await Mediator.Send(new BrandGetAll.Request());
    }
}

public static class BrandGetAll
{
    public record Request : IRequest<Response>;

    public record Response(IEnumerable<Brand> Brands);

    internal sealed class GetAllBrandsHandler : IRequestHandler<Request, Response>
    {
        private readonly IDataService _dataService;

        public GetAllBrandsHandler(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var data = await _dataService.GetAllBrandsAsync();

            return new Response(data ?? new List<Brand>());
        }
    }
}