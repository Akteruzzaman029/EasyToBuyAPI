using Core.Identity;
using Core.Model;
using Core.ModelDto.AspNetUsers;

namespace Infrastructure.IRepository
{
    public interface IAspNetUsersRepository
    {
        Task<PaginatedListModel<AspNetUsersResponseDto>> GetAspNetUserses(int pageNumber, AspNetUsersFilterDto searchModel);
        Task<List<AspNetUsersResponseDto>> GetDistinctAspNetUserses(AspNetUsersFilterDto searchModel);
        Task<List<AspNetUsersResponseDto>> GetAspNetUsersByType(int type);
        Task<AspNetUsersResponseDto> GetAspNetUsersById(string userId);
        Task<ApplicationUser> InsertAspNetUsers(AspNetUsersRequestDto insertRequestModel);
        Task<ApplicationUser> UpdateAspNetUsers(string userId, AspNetUsersRequestDto updateRequestModel);
        Task<int> ChangePasswordAsync(string userId, AspNetUsersRequestDto updateRequestModel);
        Task<int> DeleteAspNetUsers(string userId, AspNetUsersRequestDto deleteRequestModel);
    }
}
