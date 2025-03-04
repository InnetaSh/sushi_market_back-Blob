using Microsoft.AspNetCore.Mvc;

namespace _26._02_sushi_market_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : Controller
    {

  
        private List<Category> CategoriesList => categoriesByProduct.GetCategoriesList();

        private CategoriesByProduct categoriesByProduct;
        public MarketController()
        {
            categoriesByProduct = new CategoriesByProduct();
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            if (CategoriesList.Count == 0)
                return NotFound("No categories found.");
            return Ok(CategoriesList);
        }



    }
}
