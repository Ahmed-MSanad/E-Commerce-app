using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;

namespace Services.Implementation
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        {
            lazyProductService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            lazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        }

        private readonly Lazy<IProductService> lazyProductService;
        private Lazy<IBasketService> lazyBasketService;
        public IProductService ProductService => lazyProductService.Value;
        public IBasketService BasketService => lazyBasketService.Value;
    }
}
