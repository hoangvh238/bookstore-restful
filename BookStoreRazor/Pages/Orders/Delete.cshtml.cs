using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Orders
{
	public class DeleteModel : PageModel
	{
		private readonly RestClient _restClient;

		public DeleteModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		[BindProperty]
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
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized nếu không có token
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

		public async Task<IActionResult> OnPostAsync(int? id)
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

			var request = new RestRequest($"api/orders/{id}", Method.Delete);
			request.AddHeader("Authorization", $"Bearer {token}");

			var response = await _restClient.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				return RedirectToPage("./Index");
			}

			ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xóa đơn hàng.");
			return Page();
		}
	}
}
