using Ecommerce.Core.src.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class ReviewTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void UpdateRating_WithValidRating_ShouldUpdateRating(int validRating)
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Initial review context");

            // Act
            review.UpdateRating(validRating);

            // Assert
            review.Rating.Should().Be(validRating);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void UpdateRating_WithInvalidRating_ShouldThrowArgumentOutOfRangeException(int invalidRating)
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Initial review context");

            // Act
            Action act = () => review.UpdateRating(invalidRating);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Rating must be between 1 and 5. (Parameter 'newRating')");
        }

        [Fact]
        public void UpdateContext_WithValidContext_ShouldUpdateContext()
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Initial");

            // Act
            review.UpdateContext("Updated review context");

            // Assert
            review.Context.Should().Be("Updated review context");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void UpdateContext_WithInvalidContext_ShouldThrowArgumentException(string? invalidContext)
        {
            // Arrange
            var review = new Review(Guid.NewGuid(), Guid.NewGuid(), 3, "Initial");

            // Act
            Action act = () => review.UpdateContext(invalidContext!);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Review context cannot be null or empty. (Parameter 'newContext')");
        }

    }
}