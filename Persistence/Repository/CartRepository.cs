using Core.Model;
using Core.ModelDto.Cart;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public CartRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<CartResponseDto>> GetCarts(int pageNumber, CartFilterDto searchModel)
        {
            PaginatedListModel<CartResponseDto> output = new PaginatedListModel<CartResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("UserId", searchModel.UserId);
                p.Add("GuestId", searchModel.GuestId);
                p.Add("CartType", searchModel.CartType);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<CartResponseDto, dynamic>("USP_Cart_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<CartResponseDto>
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


        public async Task<List<CartResponseDto>> GetDistinctCarts(CartFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("UserId", searchModel.UserId);
            p.Add("GuestId", searchModel.GuestId);
            p.Add("CartType", searchModel.CartType);

            var output = await _dataAccessHelper.QueryData<CartResponseDto, dynamic>("USP_Cart_GetDistinct", p);

            return output;
        }

        public async Task<CartResponseDto> GetCartById(int CartId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CartId);
            return (await _dataAccessHelper.QueryData<CartResponseDto, dynamic>("USP_Cart_GetById", p)).FirstOrDefault();
        }

        public async Task<List<CartResponseDto>> GetCartsByName(CartRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("GuestId", insertRequestModel.GuestId);
            p.Add("CartType", insertRequestModel.CartType);
            p.Add("ProductId", insertRequestModel.ProductId);
            var output = await _dataAccessHelper.QueryData<CartResponseDto, dynamic>("USP_Cart_GetCartsByName", p);
            return output;
        }


        public async Task<int> InsertCart(CartRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("GuestId", insertRequestModel.GuestId);
            p.Add("CartType", insertRequestModel.CartType);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("Quantity", insertRequestModel.Quantity);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Cart_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateCart(int CartId, CartRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CartId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("GuestId", insertRequestModel.GuestId);
            p.Add("CartType", insertRequestModel.CartType);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("Quantity", insertRequestModel.Quantity);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);


            return await _dataAccessHelper.ExecuteData("USP_Cart_Update", p);
        }

        public async Task<int> DeleteCart(int CartId, CartRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CartId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);

            return await _dataAccessHelper.ExecuteData("USP_Cart_Delete", p);
        }
    }
}
