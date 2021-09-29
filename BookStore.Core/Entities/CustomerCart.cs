using System.Collections.Generic;

namespace BookStore.Core.Entities
{
    public class CustomerCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();
    }
}