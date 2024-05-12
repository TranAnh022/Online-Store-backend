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
            try
            {
                var reviews = await _reviewService.GetAllAsync(options);
                if (reviews.Any())
                {
                    return Ok(reviews);
                }
                return NotFound("No reviews available.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve reviews: {ex.Message}");
            }
        }

        // GET: api/v1/reviews/{id}
        // Get all reviews by product ID
        [HttpGet("products/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetReviewsByProduct([FromRoute] Guid id)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByProductIdAsync(id);
                if (!reviews.Any())
                {
                    return NotFound($"No reviews found for product ID: {id}");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving reviews for product ID: {id}. Error: {ex.Message}");
            }
        }

        // GET: api/v1/reviews/users/{userId}
        // Get all reviews by user ID
        [HttpGet("users/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewReadDto>>> GetReviewsByUser([FromRoute] Guid id)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByUserIdAsync(id);
                if (reviews == null || !reviews.Any())
                {
                    return NotFound("No reviews found for the specified user.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving reviews: {ex.Message}");
            }
        }

        // POST: api/v1/reviews
        // Adds a new product review
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<ReviewReadDto>> AddReview([FromBody] ReviewCreateDto reviewCreateDto)
        {
            try
            {
                var createdReview = await _reviewService.CreateOneAsync(reviewCreateDto);
                return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, createdReview);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create review: {ex.Message}");
            }
        }

        // GET: api/v1/reviews/{id}
        // Retrieves details of a specific review by its id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ReviewReadDto>> GetReviewById([FromRoute] Guid id)
        {
            try
            {
                var review = await _reviewService.GetOneByIdAsync(id);
                if (review == null)
                {
                    return NotFound($"No review found with ID: {id}");
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving review with ID: {id}. Error: {ex.Message}");
            }
        }

        // PUT: api/v1/reviews/{id}
        // Updates a specific review
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            try
            {
                var updatedReview = await _reviewService.UpdateOneAsync(id, reviewUpdateDto);
                if (updatedReview == null)
                {
                    return NotFound($"No review found with ID: {id} for update.");
                }
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update review with ID: {id}. Error: {ex.Message}");
            }
        }

        // DELETE: api/v1/reviews/{id}
        // Deletes a specific review by ID
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            try
            {
                var result = await _reviewService.DeleteOneAsync(id);
                if (!result)
                {
                    return NotFound($"No review found with ID: {id} for deletion.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete review with ID: {id}. Error: {ex.Message}");
            }
        }
    }
}