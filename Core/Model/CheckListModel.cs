namespace Core.Model
{
    public class CheckListModel : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int FileId { get; set; } = 0;
        public int Weight { get; set; } = 0;
    }
}