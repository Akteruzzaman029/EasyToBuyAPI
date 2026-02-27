namespace Core.ModelDto.Banner
{
    public class BannerFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string TypeTag { get; set; } = string.Empty;
        public int CompanyId { get; set; } 
        public bool IsActive { get; set; }
    }
}
