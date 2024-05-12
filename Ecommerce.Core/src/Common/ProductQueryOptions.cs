namespace Ecommerce.Core.src.Common
{
    public class ProductQueryOptions : QueryOptions
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Override an existing property if needed
        public override int? PageSize
        {
            get => base.PageSize;
            set => base.PageSize = value > 50 ? 50 : value;
        }
    }
}