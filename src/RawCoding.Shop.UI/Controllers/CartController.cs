using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RawCoding.Data;
using RawCoding.Data.Extensions;
using RawCoding.Shop.Application.CartActions;
using RawCoding.Shop.Domain.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace RawCoding.Shop.UI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize(Policy = PlatformConstants.Policies.Visitor)]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [AllowAnonymous]
        [HttpGet(PlatformConstants.VisitorSignInPath)]
        public async Task<IActionResult> VisitorSignIn(string returnUrl)
        {
            await HttpContext.AuthenticateVisitor();

            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartForComponent([FromServices] GetCart getCart)
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Cookie Policy not accepted");
            }

            return Ok(await getCart.GetCartForComponent(userId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(
            [FromBody] UpdateCart.Form request,
            [FromServices] UpdateCart updateCart)
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Cookie Policy not accepted");
            }

            request.UserId = userId;
            var result = await updateCart.Do(request);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpDelete("{stockId}")]
        public async Task<IActionResult> DeleteFromCart(int stockId, [FromServices] RemoveFromCart removeFromCart)
        {
            var userId = User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Cookie Policy not accepted");
            }

            var result = await removeFromCart.Do(new RemoveFromCart.Form
            {
                UserId = userId,
                StockId = stockId
            });

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }
    }
}