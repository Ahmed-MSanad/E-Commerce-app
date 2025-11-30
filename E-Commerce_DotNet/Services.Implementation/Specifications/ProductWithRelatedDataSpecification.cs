using Domain.Contracts;
using Domain.Models.Product;
using Shared.ProductDtos;

namespace Services.Implementation.Specifications
{
    public class ProductWithRelatedDataSpecification : Specification<Product>
    {
        public ProductWithRelatedDataSpecification(ProductSpecificationParams productSpecificationParams) 
            : base(product => product.Name.ToLower().Trim().Contains(productSpecificationParams.search.ToLower().Trim()))
        {
            AddInclude(product => product.ProductSubCategory);
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductImages);
        }

        public ProductWithRelatedDataSpecification(int productId)
            : base(product => product.Id == productId)
        {
            AddInclude(product => product.ProductSubCategory);
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductImages);
        }
    }
}
