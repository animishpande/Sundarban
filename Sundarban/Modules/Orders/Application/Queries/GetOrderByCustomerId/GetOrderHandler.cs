using MediatR;
using Sundarban.Modules.Orders.Domain;
using Sundarban.Modules.Orders.Infrastructure;
using Sundarban.Shared.Services;

namespace Sundarban.Modules.Orders.Application.Queries;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, List<Order>>
{
    private readonly OrdersDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public GetOrderHandler(OrdersDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    } 
    
    public async Task<List<Order>> Handle(GetOrderQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = $"order:{query.CustomerId}";
        
        // check cache
        var cached = await _cacheService.GetAsync<List<Order>>(cacheKey, cancellationToken);
        if (cached is not null) return cached;
        
        var orderList = _dbContext.Orders.Where(o => o.CustomerId == query.CustomerId).ToList();
        
        // set cache
        await _cacheService.SetAsync(cacheKey, orderList, TimeSpan.FromMinutes(5), cancellationToken);
        
        return orderList;
    }
}