namespace _26._02_sushi_market_back.Models
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }

       
        public string Count { get; set; }
        public string Title { get; set; }
    }

    public class FileUploadInfoDto
    {
        public IFormFile File { get; set; }


        public string? Category { get; set; }
        public string Title { get; set; }
        public string About_1 { get; set; }
        public string About_2 { get; set; }
        public string Price { get; set; }
    }
}
