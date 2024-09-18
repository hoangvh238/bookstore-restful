using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreRazor.Pages.Products
{
	public class IndexModel : PageModel
	{
		private readonly RestClient _restClient;

		public IndexModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public IList<Product> Products { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized nếu không có token
			}

			var request = new RestRequest("api/products", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			request.AddParameter("productType", "category");


			var response = await _restClient.ExecuteAsync<List<Product>>(request);

			if (response.IsSuccessful && response.Data != null)
			{
				Products = response.Data;
				return Page();
			}

			Products = new List<Product>();
			return Page();  // Trả về trang mặc dù không có sản phẩm
		}
	}
}
