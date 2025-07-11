using Core.ModelDto;
using Dapper;
using Infrastructure.IRepository;

namespace Persistence.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        public AuthRepository(DataAccessHelper dataAccessHelper)
        {
            _dataAccessHelper = dataAccessHelper;
        }
        public async Task<int> UpdateRefreshToken(string userId, LoginResponseDto token)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", userId);
            p.Add("RefreshToten", token.RefreshToken);
            p.Add("RefreshTokenExpires", token.RefreshTokenExpires);
            return await _dataAccessHelper.ExecuteData("USP_AspNetUsers_Token_Update", p);

        }

    }
}
