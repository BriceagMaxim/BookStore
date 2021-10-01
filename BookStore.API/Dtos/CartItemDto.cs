namespace BookStore.API.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        
        public BookDto Book { get; set; }
    }
}