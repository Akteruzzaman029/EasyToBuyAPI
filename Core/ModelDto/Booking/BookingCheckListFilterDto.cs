namespace Core.ModelDto.BookingCheckList
{
    public class BookingCheckListFilterDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CheckListName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
