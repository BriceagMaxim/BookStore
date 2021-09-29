using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}