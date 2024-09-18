using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Products
{
	public class DetailsModel : PageModel
	{
		private readonly RestClient _restClient;

		public DetailsModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public Product Product { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized nếu không có token
			}

			var request = new RestRequest($"api/products/{id}", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await _restClient.ExecuteAsync<Product>(request);

			if (response.IsSuccessful && response.Data != null)
			{
				Product = response.Data;
				return Page();
			}

			return NotFound();
		}
	}
}
