using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RawCoding.Data;

namespace RawCoding.Identity.Pages
{
    public class Register : PageModel
    {
        [BindProperty] public RegisterForm Form { get; set; }

        public void OnGet(string returnUrl)
        {
            Form = new RegisterForm
            {
                ReturnUrl = returnUrl,
            };
        }

        // todo display errors
        public async Task<IActionResult> OnPost(
            [FromServices] SignInManager<PlatformUser> signInManager,
            [FromServices] UserManager<PlatformUser> userManager)
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new PlatformUser
            {
                UserName = Form.Email,
                Email = Form.Email,
            };

            var createUserResult = await userManager.CreateAsync(user, Form.Password);
            if (!createUserResult.Succeeded)
            {
                return Page();
            }

            await signInManager.SignInAsync(user, false);

            if (Form.ReturnUrl is {Length: > 0})
            {
                return Redirect(Form.ReturnUrl);
            }

            return RedirectToPage("/Index");
        }

        public class RegisterForm
        {
            public string ReturnUrl { get; set; }
            [Required] public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password))]
            public string ConfirmPassword { get; set; }
        }
    }
}