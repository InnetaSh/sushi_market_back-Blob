using _26._02_sushi_market_back.Services;
using _26._02_sushi_market_back.Models;
using Microsoft.AspNetCore.Mvc;

namespace _26._02_sushi_market_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuOldController : Controller
    {



        private List<MenuInfoModel> ProductTitleList => titleByProduct.GetProductTitleList();

        private TitleByProductService titleByProduct;
        public MenuOldController()
        {
            titleByProduct = new TitleByProductService();
        }



        [HttpGet("search/category/{category}")]
        public ActionResult<List<MenuInfoModel>> GetByCategory(string category)
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
