using Core.Model;
using Core.ModelDto.Expenditure;

namespace Infrastructure.IRepository;

public interface IExpenditureRepository
{
    Task<PaginatedListModel<ExpenditureResponseDto>> GetExpenditures(int pageNumber, ExpenditureFilterDto searchModel);
    Task<List<ExpenditureResponseDto>> GetDistinctExpenditures();
    Task<ExpenditureResponseDto> GetExpenditureById(int ExpenditureId);
    Task<int> InsertExpenditure(ExpenditureRequestDto insertRequestModel);
    Task<int> UpdateExpenditure(int ExpenditureId, ExpenditureRequestDto updateRequestModel);
    Task<int> DeleteExpenditure(int ExpenditureId, ExpenditureRequestDto deleteRequestModel);
}
