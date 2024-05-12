using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.Entities
{
    public class Review : TimeStamp
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string? Context { get; set; }
        public User? User { get; set; }
        public Product? Product { get; set; }

        public Review() { }

        public Review(Guid userId, Guid productId, int rating, string context)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            ProductId = productId;
            Rating = rating;
            Context = context;
        }

        // Method for updating the rating of a review
        public void UpdateRating(int newRating)
        {
            Guard.Against.OutOfRange(newRating, nameof(newRating), 1, 5, "Rating must be between 1 and 5.");
            Rating = newRating;
        }

        // Method for context of a review
        public void UpdateContext(string newContext)
        {
            Guard.Against.NullOrWhiteSpace(newContext, nameof(newContext), "Review context cannot be null or empty.");
            Context = newContext;
        }

    }
}