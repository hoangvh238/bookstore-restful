using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreRazor.Pages.Categories
{
	public class IndexModel : PageModel
	{
		private readonly RestClient _restClient;

		public IndexModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public IList<Category> Categories { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized nếu không có token
			}

			var request = new RestRequest("api/categories", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await _restClient.ExecuteAsync<List<Category>>(request);

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized nếu API trả về lỗi xác thực
			}

			if (response.IsSuccessful && response.Data != null)
			{
				Categories = response.Data;
			}
			else
			{
				Categories = new List<Category>();
			}

			return Page();
		}
	}
}
