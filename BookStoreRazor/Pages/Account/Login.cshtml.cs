using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

public class LoginModel : PageModel
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

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	public class TokenData
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("user_id")]
		public int UserId { get; set; }

		[JsonProperty("role")]
		public string Role { get; set; }
	}
	public async Task<IActionResult> OnPostAsync()
	{
		if (ModelState.IsValid)
		{
			try
			{
				var client = new RestClient("https://localhost:7180/api/Users/Login");
				var request = new RestRequest();
				request.AddJsonBody(new
				{
					Email = Input.Email,
					Password = Input.Password
				});

				var response = await client.PostAsync(request);

				if (response.IsSuccessful)
				{
					var resData = response.Content; 
					var tokenData = JsonConvert.DeserializeObject<TokenData>(resData);
				

					Response.Cookies.Append("auth_token", tokenData.AccessToken, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						Expires = DateTimeOffset.UtcNow.AddDays(60)
					});
					Response.Cookies.Append("role", tokenData.Role, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						Expires = DateTimeOffset.UtcNow.AddDays(60)
					});

					Response.Cookies.Append("user_id", tokenData.UserId.ToString(), new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						Expires = DateTimeOffset.UtcNow.AddDays(60)
					});


					if (tokenData.Role != "Users")
					{
						return Redirect("/Products");
					}
					else
					{
						return Redirect("/Home");
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		return Page();
	}
}