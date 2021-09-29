using System.Collections.Generic;

namespace BookStore.Core.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}