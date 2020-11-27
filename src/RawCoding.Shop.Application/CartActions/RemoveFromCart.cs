using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Application.CartActions
{
    [Service]
    public class RemoveFromCart
    {
        private readonly ICartManager _cartManager;

        public RemoveFromCart(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public class Form
        {
            public string UserId { get; set; }
            public int StockId { get; set; }
        }

        public async Task<BaseResponse> Do(Form request)
        {
            var removedStock = await _cartManager.RemoveStock(request.StockId, request.UserId);
            if (removedStock < 0)
            {
                return new BaseResponse("Product not found", false);
            }

            return new BaseResponse("Removed from cart");
        }
    }
}