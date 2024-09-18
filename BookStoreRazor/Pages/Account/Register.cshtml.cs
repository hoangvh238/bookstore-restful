using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

public class RegisterModel : PageModel
{
	[BindProperty]
	public InputModel Input { get; set; }

	public class InputModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (ModelState.IsValid)
		{
			var client = new RestClient("https://localhost:7180/api/Users/Register");
			var request = new RestRequest();

			request.AddJsonBody(new
			{
				Email = Input.Email,
				Password = Input.Password
			});

			var response = await client.PostAsync(request);

			if (response.IsSuccessful)
			{
				return RedirectToPage("/Index");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
			}
		}
		return Page();
	}
}
