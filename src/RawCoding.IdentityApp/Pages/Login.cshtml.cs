using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RawCoding.Data;

namespace RawCoding.Identity.Pages
{
    public class Login : PageModel
    {
        [BindProperty] public LoginForm Form { get; set; }

        public void OnGet(string returnUrl)
        {
            Form = new LoginForm
            {
                ReturnUrl = returnUrl
            };
        }

        public async Task<IActionResult> OnPost(
            [FromServices] SignInManager<PlatformUser> signInManager)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await signInManager.PasswordSignInAsync(Form.Email, Form.Password, false, false);

            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(Form.ReturnUrl))
                {
                    return RedirectToPage("/Index");
                }

                return Redirect(Form.ReturnUrl);
            }

            return Page();
        }

        public class LoginForm
        {
            public string ReturnUrl { get; set; }
            [Required] public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}