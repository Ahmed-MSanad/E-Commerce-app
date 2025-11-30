using AutoMapper;
using Domain.Contracts;
using Services.Abstraction;

namespace Services.Implementation
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            lazyProductService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
        }

        private readonly Lazy<IProductService> lazyProductService;
        public IProductService ProductService => lazyProductService.Value;
    }
}
