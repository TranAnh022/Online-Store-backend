namespace Ecommerce.Core.src.Common
{
    public class QueryOptions
    {
        public virtual int? Page { get; set; } = 1;
        public virtual int? PageSize { get; set; } = 10;
        public virtual string? SortBy { get; set; }
        public virtual string? SortOrder { get; set; }
        public virtual string? Search { get; set; }
    }
}