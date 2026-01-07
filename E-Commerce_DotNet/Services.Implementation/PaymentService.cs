using AutoMapper;
using Domain.Contracts;
using Domain.Models.Basket;
using Domain.Models.Order;
using Microsoft.Extensions.Configuration;
using Services.Abstraction;
using Services.Implementation.Specifications;
using Shared.BasketDtos;
using Stripe;

namespace Services.Implementation
{
    public class PaymentService(
        IConfiguration configuration, 
        IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration.GetRequiredSection("Stripe")["SecretKey"];

            CustomerBasket basket = await basketRepository.GetBasketAsync(basketId);
            if (basket is null) throw new Exception($"Basket with id {basketId} is not found");

            // Make sure the price for basket products is itself the price stored in the Db:
            var productRepo = unitOfWork.GetRepository<Domain.Models.Product.Product, int>();
            foreach (var item in basket.Basket)
            {
                var product = await productRepo.GetAsync(item.Id);
                if (product is null) throw new Exception($"Product with id {item.Id} is not found");

                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new ArgumentException();

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null) throw new Exception($"Delivery Method with id {basket.DeliveryMethodId.Value} is not found");

            basket.ShippingPrice = deliveryMethod.Price;

            var amount = (long)(basket.Basket.Sum(item => item.Price * item.BasketQuantity) + basket.ShippingPrice) * 100;

            var service = new PaymentIntentService();
            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId)) // no payment intent for this basket then create one
            {
                var paymentIntentOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    PaymentMethodTypes = ["card"],
                    Currency = "USD"
                };
                var paymentIntent = await service.CreateAsync(paymentIntentOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepository.UpdateBasketAsync(basket);

            return mapper.Map<BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var endPointSecret = configuration.GetSection("Stripe")["WebhokSecret"];

            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, endPointSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await UpdateOrderPaymentReceivedAsync(paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdateOrderPaymentFailedAsync(paymentIntent.Id);
                    break;
            }
        }
        private async Task UpdateOrderPaymentReceivedAsync(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));

            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;

            unitOfWork.GetRepository<Order, Guid>().Update(order);

            await unitOfWork.SaveChangesAsync();
        }
        private async Task UpdateOrderPaymentFailedAsync(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));

            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;

            unitOfWork.GetRepository<Order, Guid>().Update(order);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
