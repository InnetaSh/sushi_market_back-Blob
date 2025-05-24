namespace _26._02_sushi_market_back.Models
{
    public class MenuInfoModel
    {
    public int Id { get; set; }
    public string? Category { get; set; }
    public string BlobUrl { get; set; } // URL изображения из Azure Blob
    public string Title { get; set; }
    public string About_1 { get; set; }
    public string About_2 { get; set; }
    public string Price { get; set; }

    public MenuInfoModel() { }
    }
}
