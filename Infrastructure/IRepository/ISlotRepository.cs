using Core.Model;
using Core.ModelDto.Slot;

namespace Infrastructure.IRepository;

public interface ISlotRepository
{
    Task<PaginatedListModel<SlotResponseDto>> GetSlots(int pageNumber, SlotFilterDto searchModel);
    Task<List<SlotResponseDto>> GetDistinctSlots(DateTime StartDate);
    Task<SlotResponseDto> GetSlotById(int SlotId);
    Task<SlotResponseDto> GetSlotByName(SlotRequestDto updateRequestModel);
    Task<List<dynamic>> GetMonthlySlot(DateTime startDate);
    Task<int> InsertSlot(SlotRequestDto insertRequestModel);
    Task<int> UpdateSlot(int SlotId, SlotRequestDto updateRequestModel);
    Task<int> DeleteSlot(int SlotId, SlotRequestDto deleteRequestModel);
}
