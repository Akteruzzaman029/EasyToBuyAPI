namespace Core.ModelDto.ProductComment
{
    public class ProductCommentFilterDto
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public bool IsActive { get; set; }

    }
}
