using Core.Model;
using Core.ModelDto.Address;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public AddressRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<AddressResponseDto>> GetAddresss(int pageNumber, AddressFilterDto searchModel)
        {
            PaginatedListModel<AddressResponseDto> output = new PaginatedListModel<AddressResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("UserId", searchModel.UserId);
                p.Add("PickerName", searchModel.PickerName);
                p.Add("PickerNumber", searchModel.PickerNumber);
                p.Add("StreetAddress", searchModel.StreetAddress);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<AddressResponseDto, dynamic>("USP_Address_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<AddressResponseDto>
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

        public async Task<List<AddressResponseDto>> GetDistinctAddresss(AddressFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("PickerName", searchModel.PickerName);
            p.Add("PickerNumber", searchModel.PickerNumber);
            p.Add("StreetAddress", searchModel.StreetAddress);

            var output = await _dataAccessHelper.QueryData<AddressResponseDto, dynamic>("USP_Address_GetDistinct", p);

            return output;
        }

        public async Task<AddressResponseDto> GetAddressById(int AddressId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", AddressId);
            return (await _dataAccessHelper.QueryData<AddressResponseDto, dynamic>("USP_Address_GetById", p)).FirstOrDefault();
        }

        public async Task<List<AddressResponseDto>> GetAddresssByName(AddressRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("PickerName", insertRequestModel.PickerName);
            p.Add("PickerNumber", insertRequestModel.PickerNumber);
            p.Add("StreetAddress", insertRequestModel.StreetAddress);
            var output = await _dataAccessHelper.QueryData<AddressResponseDto, dynamic>("USP_Address_GetAddresssByName", p);
            return output;
        }


        public async Task<int> InsertAddress(AddressRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("PickerName", insertRequestModel.PickerName);
            p.Add("PickerNumber", insertRequestModel.PickerNumber);
            p.Add("StreetAddress", insertRequestModel.StreetAddress);
            p.Add("Building", insertRequestModel.Building);
            p.Add("City", insertRequestModel.City);
            p.Add("State", insertRequestModel.State);
            p.Add("ZipCode", insertRequestModel.ZipCode);
            p.Add("IsDefault", insertRequestModel.IsDefault);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Address_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateAddress(int AddressId, AddressRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", AddressId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("PickerName", insertRequestModel.PickerName);
            p.Add("PickerNumber", insertRequestModel.PickerNumber);
            p.Add("StreetAddress", insertRequestModel.StreetAddress);
            p.Add("Building", insertRequestModel.Building);
            p.Add("City", insertRequestModel.City);
            p.Add("State", insertRequestModel.State);
            p.Add("ZipCode", insertRequestModel.ZipCode);
            p.Add("IsDefault", insertRequestModel.IsDefault);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_Address_Update", p);
        }

        public async Task<int> DeleteAddress(int AddressId, AddressRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", AddressId);
            p.Add("UserId", deleteRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_Address_Delete", p);
        }
    }
}
