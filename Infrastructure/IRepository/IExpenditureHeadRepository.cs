using Core.Model;
using Core.ModelDto.ExpenditureHead;

namespace Infrastructure.IRepository;

public interface IExpenditureHeadRepository
{
    Task<PaginatedListModel<ExpenditureHeadResponseDto>> GetExpenditureHeads(int pageNumber, ExpenditureHeadFilterDto searchModel);
    Task<List<ExpenditureHeadResponseDto>> GetDistinctExpenditureHeads();
    Task<ExpenditureHeadResponseDto> GetExpenditureHeadById(int ExpenditureHeadId);
    Task<int> InsertExpenditureHead(ExpenditureHeadRequestDto insertRequestModel);
    Task<int> UpdateExpenditureHead(int ExpenditureHeadId, ExpenditureHeadRequestDto updateRequestModel);
    Task<int> DeleteExpenditureHead(int ExpenditureHeadId, ExpenditureHeadRequestDto deleteRequestModel);
}
