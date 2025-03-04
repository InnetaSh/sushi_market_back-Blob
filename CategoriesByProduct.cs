using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace _26._02_sushi_market_back
{
    public class CategoriesByProduct
    {

        private List<Category> CategoriesList;

        public CategoriesByProduct()
        {
            CategoriesList = LoadCategoriesList();
        }
        private List<Category> LoadCategoriesList()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "categoriesList.json");

            var json = System.IO.File.ReadAllText("categoriesList.json");
            return JsonConvert.DeserializeObject<List<Category>>(json) ?? new List<Category>();


        }


        public void SaveCategoriesList()
        {
            var json = JsonConvert.SerializeObject(CategoriesList, Formatting.Indented);
            System.IO.File.WriteAllText("categoriesList.json", json);
        }

        public List<Category> GetCategoriesList()
        {
            return CategoriesList;
        }
    }


    public class Category
    {
        //[JsonPropertyName("id")]
        public int Id { get; set; }

        //[JsonPropertyName("imgSrc")]
        public string ImgSrc { get; set; }

        //[JsonPropertyName("count")]
        public string Count { get; set; }

        //[JsonPropertyName("title")]
        public string Title { get; set; }

        public Category() { }

        public Category(int id, string imgSrc, string count, string title)
        {
            Id = id;
            ImgSrc = imgSrc;
            Count = count;
            Title = title;
        }
    }
}
