using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Data;
using RawCoding.Shop.Application.Admin.Products;

namespace RawCoding.Shop.UI.Controllers
{
    [Route("/api/products")]
    [ApiController]
    [Authorize(Policy = PlatformConstants.Policies.Visitor)]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<object> GetProducts([FromServices] GetProducts getProducts)
        {
            return getProducts.FrontPage();
        }
    }
}