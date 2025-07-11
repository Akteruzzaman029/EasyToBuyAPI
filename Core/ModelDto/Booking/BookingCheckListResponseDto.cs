using Core.Model;
using Microsoft.VisualBasic.FileIO;
using static Core.BaseEnum;

namespace Core.ModelDto.BookingCheckList
{
    public class BookingCheckListResponseDto : BookingCheckListModel
    {
        public string CheckListName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
