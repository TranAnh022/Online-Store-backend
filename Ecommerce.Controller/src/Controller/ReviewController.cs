using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/v1/reviews
        // Retrieves all reviews with optional pagination and sorting
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetAllReviews([FromQuery] QueryOptions options)
        {
            var reviews = await _reviewService.GetAllAsync(options);
            return Ok(reviews);
        }
        // GET: api/v1/reviews/{id}
        // Get all reviews by product ID
        [HttpGet("products/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetReviewsByProduct([FromRoute] Guid id)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(id);
            return Ok(reviews);
        }

        // POST: api/v1/reviews
        // Adds a new product review
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<ReviewReadDto>> AddReview([FromBody] ReviewCreateDto reviewCreateDto)
        {
            var createdReview = await _reviewService.CreateOneAsync(reviewCreateDto);
            return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, createdReview);
        }

        // GET: api/v1/reviews/{id}
        // Retrieves details of a specific review by its id
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewReadDto>> GetReviewById([FromRoute] Guid id)
        {
            var review = await _reviewService.GetOneByIdAsync(id);
            return Ok(review);
        }

        // PUT: api/v1/reviews/{id}
        // Updates a specific review
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            var updatedReview = await _reviewService.UpdateOneAsync(id, reviewUpdateDto);
            return Ok(updatedReview);
        }

        // DELETE: api/v1/reviews/{id}
        // Deletes a specific review by ID
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            var result = await _reviewService.DeleteOneAsync(id);
            return Ok(result);
        }

    }
}