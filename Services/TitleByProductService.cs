using Newtonsoft.Json;
using System.Text.Json.Serialization;
using _26._02_sushi_market_back.Models;

namespace _26._02_sushi_market_back.Services
{
    public class TitleByProductService
    {




        private List<MenuInfoModel> ProductTitleList;

        public TitleByProductService()
        {
            ProductTitleList = LoadProductTitleList();
        }
        private List<MenuInfoModel> LoadProductTitleList()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "productTitleList.json");

            var json = File.ReadAllText("productTitleList.json");
            return JsonConvert.DeserializeObject<List<MenuInfoModel>>(json) ?? new List<MenuInfoModel>();


        }


        public void SaveProductTitleList()
        {
            var json = JsonConvert.SerializeObject(ProductTitleList, Formatting.Indented);
            File.WriteAllText("productTitleList.json", json);
        }

        public List<MenuInfoModel> GetProductTitleList()
        {
            return ProductTitleList;
        }
    }


}
