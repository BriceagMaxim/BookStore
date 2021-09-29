using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Core.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}