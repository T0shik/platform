// using System;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using RawCoding.Data;
// using RawCoding.Shop.Application.Emails;
// using RawCoding.Shop.Database;
// todo move to identityApp
// namespace RawCoding.Shop.UI.Controllers.Admin
// {
//     [Authorize(PlatformConstants.Policies.Admin)]
//     public class UsersController : AdminBaseController
//     {
//         private readonly UserManager<IdentityUser> _userManager;
//
//         public UsersController(UserManager<IdentityUser> userManager)
//         {
//             _userManager = userManager;
//         }
//
//         [HttpGet]
//         public async Task<IActionResult> ListManagers([FromServices] ApplicationDbContext ctx)
//         {
//             var users = await _userManager
//                 .GetUsersForClaimAsync(PlatformConstants.Shop.ManagerClaim);
//
//             return Ok(users.Select(x => new
//             {
//                 x.Id,
//                 x.UserName,
//                 x.Email,
//             }));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> CreateUser(
//             string email,
//             [FromServices] IEmailSink emailSink,
//             [FromServices] IEmailTemplateFactory emailTemplateFactory)
//         {
//             var user = new IdentityUser(email)
//             {
//                 Email = email,
//             };
//
//             var password = Enumerable.Range(0, 10)
//                 .Aggregate("", (a, b) => $"{a}{new Random().Next(b)}");
//
//             var createUserResult = await _userManager.CreateAsync(user, $"aA1!{password}");
//             if (!createUserResult.Succeeded)
//             {
//                 return BadRequest("Failed to create User");
//             }
//
//             await _userManager.AddClaimAsync(user, PlatformConstants.Shop.ManagerClaim);
//             var code = await _userManager.GeneratePasswordResetTokenAsync(user);
//
//             var link = Url.Page("/Admin/Register", "Get", new {email, code}, protocol: HttpContext.Request.Scheme);
//             await emailSink.SendAsync(new SendEmailRequest
//             {
//                 To = email,
//                 Subject = "Raw Coding - Account Activation",
//                 Message = await emailTemplateFactory.RenderRegisterInvitationAsync(link),
//                 Html = true,
//             });
//
//             return Ok();
//         }
//
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> ListManagers(string id)
//         {
//             var user = await _userManager.FindByIdAsync(id);
//             if (user == null)
//             {
//                 return NoContent();
//             }
//
//             await _userManager.DeleteAsync(user);
//             return Ok();
//         }
//     }
// }