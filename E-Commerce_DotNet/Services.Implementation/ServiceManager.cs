using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared.IdentityDtos;

namespace Services.Implementation
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, UserManager<User> userManager,
            IOptions<JwtOptions> options, IConfiguration configuration, ICacheRepository cacheRepository, IOptions<JsonOptions> jsonOptions)
        {
            lazyProductService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            lazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, options, mapper));
            lazyOrderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork, mapper, basketRepository));
            lazyPaymentService = new Lazy<IPaymentService>(() => new PaymentService(configuration, basketRepository, unitOfWork, mapper));
            lazyCacheService = new Lazy<ICacheService>(() => new CacheService(cacheRepository, jsonOptions));
        }

        private readonly Lazy<IProductService> lazyProductService;
        private readonly Lazy<IBasketService> lazyBasketService;
        private readonly Lazy<IAuthenticationService> lazyAuthenticationService;
        private readonly Lazy<IOrderService> lazyOrderService;
        private readonly Lazy<IPaymentService> lazyPaymentService;
        private readonly Lazy<ICacheService> lazyCacheService;

        public IProductService ProductService => lazyProductService.Value;
        public IBasketService BasketService => lazyBasketService.Value;
        public IAuthenticationService AuthenticationService => lazyAuthenticationService.Value;
        public IOrderService OrderService => lazyOrderService.Value;
        public IPaymentService PaymentService => lazyPaymentService.Value;

        public ICacheService CacheService => lazyCacheService.Value;
    }
}
