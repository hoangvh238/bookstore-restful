using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreRazor.Pages.Orders
{
	public class IndexModel : PageModel
	{
		private readonly RestClient _restClient;

		public IndexModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public IList<Order> Order { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();
			}

			var request = new RestRequest("api/orders", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await _restClient.ExecuteAsync<List<Order>>(request);

			if (response.IsSuccessful && response.Data != null)
			{
				Order = response.Data;
			}
			else
			{
				Order = new List<Order>();
			}
			return Page();
		}
	}
}
