using Domain.Contracts;
using Domain.Models.Product;
using Shared.ProductDtos;

namespace Services.Implementation.Specifications
{
    public class ProductWithTotalCountSpecification : Specification<Product>
    {
        public ProductWithTotalCountSpecification(ProductSpecificationParams productSpecificationParams)
            : base(product => (string.IsNullOrWhiteSpace(productSpecificationParams.search) || product.Name.ToLower().Trim().Contains(productSpecificationParams.search.ToLower().Trim())))
        {

        }
    }
}
