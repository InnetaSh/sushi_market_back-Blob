using _26._02_sushi_market_back.Models;
using _26._02_sushi_market_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace _26._02_sushi_market_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuInfoController : ControllerBase
    {
        private readonly BlobService _blobService;
        private readonly DatabaseMenuInfoService _db;

        public MenuInfoController(BlobService blobService, DatabaseMenuInfoService db)
        {
            _blobService = blobService;
            _db = db;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileUploadInfoDto dto)
        {
            var file = dto.File;

            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var url = await _blobService.UploadFileAsync(file);

            var menu = new MenuInfoModel
            {
                Category = dto.Category,
                BlobUrl = url,
                Title = dto.Title,
                About_1 = dto.About_1,
                About_2 = dto.About_2,
                Price = dto.Price
            };

            _db.SaveProduct(menu);

            return Ok(menu);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var menu = _db.GetProducts();
            return Ok(menu);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _db.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("search/category/{category}")]
        public IActionResult GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category is required.");
            }
            var products = _db.GetProductByCategory(category);
           
            if (products.Count > 0)
            {
                return Ok(products);
            }

            return NotFound(new { message = "Category not found" });
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] FileUploadInfoDto dto)
        {
            var existing = _db.GetProductById(id);
            if (existing == null)
                return NotFound();

            string? url = existing.BlobUrl;

            if (dto.File != null && dto.File.Length > 0)
            {
               
                url = await _blobService.UploadFileAsync(dto.File);

               
                await _blobService.DeleteFileAsync(existing.BlobUrl);
            }


            existing.Category = dto.Category;
            existing.BlobUrl = url;
            existing.Title = dto.Title;
            existing.About_1 = dto.About_1;
            existing.About_2 = dto.About_2;
            existing.Price = dto.Price;

            _db.UpdateProduct(existing);

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = _db.GetProductById(id);
            if (menuItem == null)
                return NotFound();

            await _blobService.DeleteFileAsync(menuItem.BlobUrl);

            _db.DeleteProduct(id);
            return NoContent();
        }
    }
}
