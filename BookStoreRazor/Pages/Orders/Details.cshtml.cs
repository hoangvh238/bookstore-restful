using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Orders
{
	public class DetailsModel : PageModel
	{
		private readonly RestClient _restClient;

		// Constructor nhận RestClient thông qua Dependency Injection
		public DetailsModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public Order Order { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var token = Request.Cookies["auth_token"]; 

			if (string.IsNullOrEmpty(token))
			{
				return RedirectToPage("/Account/Login");
			}

			var request = new RestRequest($"api/orders/{id}", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await _restClient.ExecuteAsync<Order>(request);

			if (response.IsSuccessful && response.Data != null)
			{
				Order = response.Data;
				return Page();
			}

			return NotFound();
		}
	}
}
