namespace Core.ModelDto.BookingCheckList
{
    public class BookingCheckListRequestDto
    {
        public int BookingId { get; set; }
        public int CheckListId { get; set; }
        public string CheckListName { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
