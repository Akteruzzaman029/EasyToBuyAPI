using Core.Model;
using Core.ModelDto.FinalTestResult;

namespace Infrastructure.IRepository;

public interface IFinalTestResultRepository
{
    Task<PaginatedListModel<FinalTestResultResponseDto>> GetFinalTestResults(int pageNumber, FinalTestResultFilterDto searchModel);
    Task<List<FinalTestResultResponseDto>> GetDistinctFinalTestResults(int postId);
    Task<FinalTestResultResponseDto> GetFinalTestResultById(int FinalTestResultId);
    Task<int> InsertFinalTestResult(FinalTestResultRequestDto insertRequestModel);
    Task<int> UpdateFinalTestResult(int FinalTestResultId, FinalTestResultRequestDto updateRequestModel);
    Task<int> DeleteFinalTestResult(int FinalTestResultId, FinalTestResultRequestDto deleteRequestModel);
}
