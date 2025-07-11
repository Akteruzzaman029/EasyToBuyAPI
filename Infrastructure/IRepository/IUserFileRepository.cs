using Core.Model;
using Core.ModelDto.UserFile;

namespace Infrastructure.IRepository;

public interface IUserFileRepository
{
    Task<PaginatedListModel<UserFileResponseDto>> GetUserFiles(int pageNumber, UserFileFilterDto searchModel);
    Task<List<UserFileResponseDto>> GetDistinctUserFiles(string userId);
    Task<UserFileResponseDto> GetUserFileById(int userfileId);
    Task<int> InsertUserFile(UserFileRequestDto insertRequestModel);
    Task<int> UpdateUserFile(int userfileId, UserFileRequestDto updateRequestModel);
    Task<int> DeleteUserFile(int userfileId, UserFileRequestDto deleteRequestModel);
}
