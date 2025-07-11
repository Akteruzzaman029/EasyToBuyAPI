using Core.Model;
using Core.ModelDto.SlotAssignment;

namespace Infrastructure.IRepository;

public interface ISlotAssignmentRepository
{
    Task<PaginatedListModel<SlotAssignmentResponseDto>> GetSlotAssignments(int pageNumber, SlotAssignmentFilterDto searchModel);
    Task<List<SlotAssignmentResponseDto>> GetDistinctSlotAssignments(int postId);
    Task<SlotAssignmentResponseDto> GetSlotAssignmentById(int SlotAssignmentId);
    Task<SlotAssignmentResponseDto> GetSlotAssignmentByName(SlotAssignmentRequestDto insertRequestModel);
    Task<int> InsertSlotAssignment(SlotAssignmentRequestDto insertRequestModel);
    Task<int> UpdateSlotAssignment(int SlotAssignmentId, SlotAssignmentRequestDto updateRequestModel);
    Task<int> DeleteSlotAssignment(int SlotAssignmentId, SlotAssignmentRequestDto deleteRequestModel);
}
