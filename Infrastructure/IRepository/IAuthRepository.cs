using Core.ModelDto;

namespace Infrastructure.IRepository
{
    public interface IAuthRepository
    {
        Task<int> UpdateRefreshToken(string userId, LoginResponseDto token);
    }
}
