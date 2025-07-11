using Core.Model;
using Core.ModelDto.CheckList;

namespace Infrastructure.IRepository;

public interface ICheckListRepository
{
    Task<PaginatedListModel<CheckListResponseDto>> GetCheckLists(int pageNumber, CheckListFilterDto searchModel);
    Task<List<CheckListResponseDto>> GetDistinctCheckLists();
    Task<CheckListResponseDto> GetCheckListById(int CheckListId);
    Task<int> InsertCheckList(CheckListRequestDto insertRequestModel);
    Task<int> UpdateCheckList(int CheckListId, CheckListRequestDto updateRequestModel);
    Task<int> DeleteCheckList(int CheckListId, CheckListRequestDto deleteRequestModel);
}
