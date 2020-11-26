using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Data;

namespace RawCoding.Shop.UI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(PlatformConstants.Shop.Policies.ShopManager)]
    public class AdminBaseController : ControllerBase
    {
    }
}