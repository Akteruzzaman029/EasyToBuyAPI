using Core.Model;
using Core.ModelDto.Address;

namespace Infrastructure.IRepository;

public interface IAddressRepository
{
    Task<PaginatedListModel<AddressResponseDto>> GetAddresss(int pageNumber, AddressFilterDto searchModel);
    Task<List<AddressResponseDto>> GetDistinctAddresss(AddressFilterDto searchModel);
    Task<AddressResponseDto> GetAddressById(int AddressId);
    Task<List<AddressResponseDto>> GetAddresssByName(AddressRequestDto insertRequestModel);
    Task<int> InsertAddress(AddressRequestDto insertRequestModel);
    Task<int> UpdateAddress(int AddressId, AddressRequestDto updateRequestModel);
    Task<int> DeleteAddress(int AddressId, AddressRequestDto deleteRequestModel);
}
