using Core.Identity;
using Core.Model;
using Core.ModelDto.AspNetUsers;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Core.BaseEnum;

namespace Persistence.Repository
{
    public class AspNetUsersRepository : IAspNetUsersRepository
    {

        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AspNetUsersRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dataAccessHelper = dataAccessHelper;
            _config = config;

        }

        public async Task<PaginatedListModel<AspNetUsersResponseDto>> GetAspNetUserses(int pageNumber, AspNetUsersFilterDto searchModel)
        {
            PaginatedListModel<AspNetUsersResponseDto> output = new PaginatedListModel<AspNetUsersResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<AspNetUsersResponseDto, dynamic>("USP_AspNetUsers_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<AspNetUsersResponseDto>
                {
                    PageIndex = pageNumber,
                    TotalRecords = TotalRecords,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages,
                    Items = result.ToList()
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return output;
        }

        public async Task<List<AspNetUsersResponseDto>> GetDistinctAspNetUserses(AspNetUsersFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            var output = await _dataAccessHelper.QueryData<AspNetUsersResponseDto, dynamic>("USP_AspNetUsers_GetDistinct", p);
            return output;
        } 
        
        public async Task<List<AspNetUsersResponseDto>> GetAspNetUsersByType(int type)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Type", type);
            var output = await _dataAccessHelper.QueryData<AspNetUsersResponseDto, dynamic>("USP_AspNetUsers_GetByType", p);
            return output;
        }

        public async Task<AspNetUsersResponseDto> GetAspNetUsersById(string userId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", userId);
            return (await _dataAccessHelper.QueryData<AspNetUsersResponseDto, dynamic>("USP_AspNetUsers_GetById", p)).FirstOrDefault();
        }


        public async Task<ApplicationUser> InsertAspNetUsers(AspNetUsersRequestDto insertRequestModel)
        {
            var user = new ApplicationUser();
            try
            {
                 user = new ApplicationUser
                {
                    UserName = insertRequestModel.Email,
                    FullName = insertRequestModel.FullName,
                    Email = insertRequestModel.Email,
                    PhoneNumber = insertRequestModel.PhoneNumber,
                    Type = insertRequestModel.Type,
                    CompanyId = insertRequestModel.CompanyId,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                };

                var result = await _userManager.CreateAsync(user, insertRequestModel.Password);
                if (result.Succeeded)
                {
                    string role= Enum.GetName(typeof(UserRoleEnum), insertRequestModel.Type) ?? string.Empty;
                    await _userManager.AddToRoleAsync(user, role);
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", insertRequestModel.FullName));
                    var usersData = await _userManager.FindByNameAsync(user.UserName);
                    return usersData;
                }
                else
                {
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException ==null ? ex.Message : ex.InnerException.Message);
            }

        }

        public async Task<ApplicationUser> UpdateAspNetUsers(string userId, AspNetUsersRequestDto updateRequestModel)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Update fields
            user.FullName = updateRequestModel.FullName;
            user.Email = updateRequestModel.Email;
            user.PhoneNumber = updateRequestModel.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("User Not updated");

            return await _userManager.FindByNameAsync(user.UserName);
        }
        public async Task<int> ChangePasswordAsync(string userId, AspNetUsersRequestDto updateRequestModel)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Ensure the old password is provided in the request
            var result = await _userManager.ChangePasswordAsync(user, updateRequestModel.CurrentPassword, updateRequestModel.Password);

            if (!result.Succeeded)
                throw new Exception("Password not updated");

            return 1;
        }

        public async Task<int> DeleteAspNetUsers(string userId, AspNetUsersRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", userId);
            return await _dataAccessHelper.ExecuteData("USP_AspNetUsers_Delete", p);
        }

        public async Task<List<AspNetUsersResponseDto>> Export(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
