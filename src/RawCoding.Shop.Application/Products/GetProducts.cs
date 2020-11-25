using System.Collections.Generic;
using System.Linq;
using RawCoding.Shop.Domain.Extensions;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.Products
{
    [Service]
    public class GetProducts
    {
        private readonly IProductManager _productManager;

        public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<object> Do()
        {
            return _productManager.GetFrontPageProducts();
        }
    }
}