namespace Core.ModelDto.Slot
{
    public class SlotFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
