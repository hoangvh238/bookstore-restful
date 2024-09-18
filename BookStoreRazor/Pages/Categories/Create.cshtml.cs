using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;
using System.Text.Json;

namespace BookStoreRazor.Pages.Categories
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
		public Category Category { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{
			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return RedirectToPage("./Unauthorized");
			}

			var request = new RestRequest("api/categories", Method.Post);
			request.AddHeader("Authorization", $"Bearer {token}");

			// Set the request to be multipart/form-data
			request.AlwaysMultipartFormData = true;

			// Add category properties
			request.AddParameter("Name", Category.Name);

			// Add the image file (if provided)
			if (Category.Image != null)
			{
				using (var ms = new MemoryStream())
				{
					await Category.Image.CopyToAsync(ms);
					var imageBytes = ms.ToArray();
					request.AddFile("Image", imageBytes, Category.Image.FileName, Category.Image.ContentType);
				}
			}

			var response = await _restClient.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				return RedirectToPage("./Index");
			}

			ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo danh mục.");
			return Page();
		}

	}
}
