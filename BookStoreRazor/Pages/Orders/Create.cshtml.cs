using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Orders
{
	public class CreateModel : PageModel
	{
		private readonly RestClient _restClient;

		public CreateModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		[BindProperty]
		public Order Order { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized
			}

			var request = new RestRequest("api/orders", Method.Post);
			request.AddHeader("Authorization", $"Bearer {token}");
			request.AddJsonBody(Order);

			var response = await _restClient.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				return RedirectToPage("./Index");
			}

			ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo đơn hàng.");
			return Page();
		}
	}
}
