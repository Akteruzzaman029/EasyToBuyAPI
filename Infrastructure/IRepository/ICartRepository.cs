using Core.Model;
using Core.ModelDto.Cart;

namespace Infrastructure.IRepository;

public interface ICartRepository
{
    Task<PaginatedListModel<CartResponseDto>> GetCarts(int pageNumber, CartFilterDto searchModel);
    Task<List<CartResponseDto>> GetDistinctCarts(CartFilterDto searchModel);
    Task<CartResponseDto> GetCartById(int CartId);
    Task<List<CartResponseDto>> GetCartsByName(CartRequestDto insertRequestModel);
    Task<int> InsertCart(CartRequestDto insertRequestModel);
    Task<int> UpdateCart(int CartId, CartRequestDto updateRequestModel);
    Task<int> DeleteCart(int CartId, CartRequestDto deleteRequestModel);
}
