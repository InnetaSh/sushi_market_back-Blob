using Microsoft.AspNetCore.Mvc;

namespace _26._02_sushi_market_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : Controller
    {
        private List<Product> ProductTitleList => titleByProduct.GetProductTitleList();

        private TitleByProduct titleByProduct;
        public MenuController()
        {
            titleByProduct = new TitleByProduct();
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            if (ProductTitleList.Count == 0)
                return NotFound("No categories found.");
            return Ok(ProductTitleList);
        }



        [HttpGet("search/category/{category}")]
        public ActionResult<List<Product>> GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category is required.");
            }

            var filteredProducts = ProductTitleList
             .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
             .ToList();

            if (filteredProducts.Count > 0)
            {
                return Ok(filteredProducts);
            }

            return NotFound(new { message = "Category not found" });
        }

    }
}
