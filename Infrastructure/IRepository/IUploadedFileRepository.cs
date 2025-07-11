using Core.Model;
using Core.ModelDto.UploadedFile;

namespace Infrastructure.IRepository;

public interface IUploadedFileRepository
{
    Task<PaginatedListModel<UploadedFileResponseDto>> GetUploadedFiles(int pageNumber, UploadedFileFilterDto searchModel);
    Task<List<UploadedFileResponseDto>> GetDistinctUploadedFiles(string userId);
    Task<UploadedFileResponseDto> GetUploadedFileById(int UploadedFileId);
    Task<int> InsertUploadedFile(UploadedFileRequestDto insertRequestModel);
    Task<int> UpdateUploadedFile(int UploadedFileId, UploadedFileRequestDto updateRequestModel);
    Task<int> DeleteUploadedFile(int UploadedFileId, UploadedFileRequestDto deleteRequestModel);
}
