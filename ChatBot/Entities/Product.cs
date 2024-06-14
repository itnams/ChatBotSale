namespace ChatBot.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ProductTypeID { get; set; }
        public string? Price { get; set; }
        public string? Configuration { get; set; }
        public int? Quantity { get; set; }
    }
}
