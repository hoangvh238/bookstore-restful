using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using BookStoreRazor.Models;
using System.Text.Json;

namespace BookStoreRazor.Pages.Products
{
	public class CreateModel : PageModel
	{
		private readonly RestClient _restClient;

		public CreateModel(RestClient restClient)
		{
			_restClient = restClient;
		}

		[BindProperty]
		public Product Product { get; set; } = default!;

		public IList<Category> Categories { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();  // Trả về mã trạng thái 401 nếu không có token
			}

			// Lấy danh mục sản phẩm từ API
			var requestCate = new RestRequest("api/categories", Method.Get);
			requestCate.AddHeader("Authorization", $"Bearer {token}");

			var responseCate = await _restClient.ExecuteAsync<List<Category>>(requestCate);

			if (responseCate.IsSuccessful && responseCate.Data != null)
			{
				Categories = responseCate.Data;
			}
			else
			{
				Categories = new List<Category>();
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{

			var token = Request.Cookies["auth_token"];
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized();  // Trả về mã trạng thái 401 Unauthorized nếu không có token
			}

			var request = new RestRequest("api/products", Method.Post);
			request.AddHeader("Authorization", $"Bearer {token}");
			request.AddJsonBody(JsonSerializer.Serialize(Product));

			// Set the request to be multipart/form-data
			request.AlwaysMultipartFormData = true;

			// Add category properties
			request.AddParameter("Name", Product.Name);
			request.AddParameter("Price", Product.Price);
			request.AddParameter("IsBestSelling", Product.IsBestSelling);
			request.AddParameter("Detail", Product.Detail);
			request.AddParameter("IsTrending", Product.IsTrending);
			request.AddParameter("IsTrending", Product.IsTrending);
			request.AddParameter("CategoryId", Product.CategoryId);



			// Add the image file (if provided)
			if (Product.Image != null)
			{
				using (var ms = new MemoryStream())
				{
					await Product.Image.CopyToAsync(ms);
					var imageBytes = ms.ToArray();
					request.AddFile("Image", imageBytes, Product.Image.FileName, Product.Image.ContentType);
				}
			}

			var response = await _restClient.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				return RedirectToPage("./Index");
			}

			ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo sản phẩm.");
			return Page();
		}
	}
}
