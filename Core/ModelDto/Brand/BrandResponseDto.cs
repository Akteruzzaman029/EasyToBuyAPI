using Core.Model;

namespace Core.ModelDto.Brand
{
    public class BrandResponseDto : BrandModel
    {
        public int ProductCount { get; set; } = 0;
        public bool IsChecked { get; set; } = false;
    }
}
