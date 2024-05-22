using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entities;
using Ecommerce.Service.DTO;

namespace Ecommerce.Service.src.DTO
{
    public class ReviewReadDto
    {
        public Guid Id { get; set; }
        public UserReadDto User { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string? Context { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReviewCreateDto
    {
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string? Context { get; set; }
    }

    public class ReviewUpdateDto
    {
        public int? Rating { get; set; }
        public string? Context { get; set; }
    }

    public class ReviewDeleteDto
    {
        public Guid Id { get; set; }
    }
}