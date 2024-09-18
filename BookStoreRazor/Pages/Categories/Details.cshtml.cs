using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Categories
{
	public class DetailsModel : PageModel
	{
		private readonly RestClient _restClient;

		public DetailsModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public Category Category { get; set; } = default!;

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

			var request = new RestRequest($"api/categories/{id}", Method.Get);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await _restClient.ExecuteAsync<Category>(request);

			if (response.IsSuccessful && response.Data != null)
			{
				Category = response.Data;
				return Page();
			}

			return NotFound();
		}
	}
}
