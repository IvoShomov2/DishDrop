using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using DishDrop.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace DishDrop.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel(UserManager<ApplicationUser> users,
    SignInManager<ApplicationUser> signIn, IEmailSender emailSender) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();
    public string ReturnUrl { get; set; } = "/";

    public class InputModel
    {
        [Required, StringLength(100)] public string FirstName { get; set; } = "";
        [Required, StringLength(100)] public string LastName { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; } = "";
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = "";
    }

    public void OnGet(string? returnUrl = null) => ReturnUrl = LocalReturnUrl(returnUrl);

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = LocalReturnUrl(returnUrl);
        if (!ModelState.IsValid) return Page();
        var user = new ApplicationUser
        {
            UserName = Input.Email, Email = Input.Email,
            FirstName = Input.FirstName.Trim(), LastName = Input.LastName.Trim()
        };
        if (user.FirstName.Length == 0 || user.LastName.Length == 0)
        {
            ModelState.AddModelError("", "First and last name are required.");
            return Page();
        }
        var result = await users.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            return Page();
        }
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await users.GenerateEmailConfirmationTokenAsync(user)));
        var callback = Url.Page("/Account/ConfirmEmail", null,
            new { area = "Identity", userId = user.Id, code, returnUrl = ReturnUrl }, Request.Scheme)!;
        await emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            $"Confirm your account by <a href='{HtmlEncoder.Default.Encode(callback)}'>clicking here</a>.");
        if (users.Options.SignIn.RequireConfirmedAccount)
            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = ReturnUrl });
        await signIn.SignInAsync(user, false);
        return LocalRedirect(ReturnUrl);
    }

    private string LocalReturnUrl(string? value) => value is not null && Url.IsLocalUrl(value) ? value : "/";
}
