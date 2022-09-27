namespace Common.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        public string Category { get; set; }
        public string StartingPrice { get; set; }
        public string BidEndDate { get; set; }
        public string SellerId { get; set; }
    }
}
