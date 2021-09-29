namespace BookStore.Core.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string AuthorName { get; set; }
    }
}