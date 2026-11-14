using System.ComponentModel.DataAnnotations;
using DishDrop.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DishDrop.Web.Areas.Identity.Pages.Account.Manage;

[Authorize]
public class IndexModel(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();
    [TempData] public string? StatusMessage { get; set; }
    public class InputModel
    {
        [Required, StringLength(100)] public string FirstName { get; set; } = "";
        [Required, StringLength(100)] public string LastName { get; set; } = "";
        [Phone, StringLength(30)] public string? PhoneNumber { get; set; }
    }
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await users.GetUserAsync(User);
        if (user is null) return NotFound();
        Input = new() { FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber };
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await users.GetUserAsync(User);
        if (user is null) return NotFound();
        if (!ModelState.IsValid) return Page();
        user.FirstName = Input.FirstName.Trim();
        user.LastName = Input.LastName.Trim();
        if (user.FirstName.Length == 0 || user.LastName.Length == 0)
        {
            ModelState.AddModelError("", "First and last name are required.");
            return Page();
        }
        if (user.PhoneNumber != Input.PhoneNumber)
        {
            user.PhoneNumber = Input.PhoneNumber;
            user.PhoneNumberConfirmed = false;
        }
        var result = await users.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            return Page();
        }
        await signIn.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated.";
        return RedirectToPage();
    }
}
