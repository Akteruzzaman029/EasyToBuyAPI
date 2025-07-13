using Core.Model;
using Core.ModelDto.MeasurementUnit;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class MeasurementUnitRepository : IMeasurementUnitRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public MeasurementUnitRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<MeasurementUnitResponseDto>> GetMeasurementUnits(int pageNumber, MeasurementUnitFilterDto searchModel)
        {
            PaginatedListModel<MeasurementUnitResponseDto> output = new PaginatedListModel<MeasurementUnitResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("UnitName", searchModel.UnitName);
                p.Add("UnitType", searchModel.UnitType);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<MeasurementUnitResponseDto, dynamic>("USP_MeasurementUnit_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<MeasurementUnitResponseDto>
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


        public async Task<List<MeasurementUnitResponseDto>> GetDistinctMeasurementUnits(MeasurementUnitFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("UnitName", searchModel.UnitName);
            p.Add("UnitType", searchModel.UnitType);

            var output = await _dataAccessHelper.QueryData<MeasurementUnitResponseDto, dynamic>("USP_MeasurementUnit_GetDistinct", p);

            return output;
        }

        public async Task<MeasurementUnitResponseDto> GetMeasurementUnitById(int MeasurementUnitId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", MeasurementUnitId);
            return (await _dataAccessHelper.QueryData<MeasurementUnitResponseDto, dynamic>("USP_MeasurementUnit_GetById", p)).FirstOrDefault();
        }

        public async Task<List<MeasurementUnitResponseDto>> GetMeasurementUnitsByName(MeasurementUnitRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UnitName", insertRequestModel.UnitName);
            p.Add("UnitType", insertRequestModel.UnitType);
            var output = await _dataAccessHelper.QueryData<MeasurementUnitResponseDto, dynamic>("USP_MeasurementUnit_GetMeasurementUnitsByName", p);
            return output;
        }


        public async Task<int> InsertMeasurementUnit(MeasurementUnitRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UnitName", insertRequestModel.UnitName);
            p.Add("Symbol", insertRequestModel.Symbol);
            p.Add("UnitType", insertRequestModel.UnitType);
            p.Add("Note", insertRequestModel.Note);
            p.Add("IsRound", insertRequestModel.IsRound);
            p.Add("IsSmallUnit", insertRequestModel.IsSmallUnit);
            p.Add("SymbolInBangla", insertRequestModel.SymbolInBangla);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_MeasurementUnit_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateMeasurementUnit(int MeasurementUnitId, MeasurementUnitRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", MeasurementUnitId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UnitName", insertRequestModel.UnitName);
            p.Add("Symbol", insertRequestModel.Symbol);
            p.Add("UnitType", insertRequestModel.UnitType);
            p.Add("Note", insertRequestModel.Note);
            p.Add("IsRound", insertRequestModel.IsRound);
            p.Add("IsSmallUnit", insertRequestModel.IsSmallUnit);
            p.Add("SymbolInBangla", insertRequestModel.SymbolInBangla);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);


            return await _dataAccessHelper.ExecuteData("USP_MeasurementUnit_Update", p);
        }

        public async Task<int> DeleteMeasurementUnit(int MeasurementUnitId, MeasurementUnitRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", MeasurementUnitId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);

            return await _dataAccessHelper.ExecuteData("USP_MeasurementUnit_Delete", p);
        }
    }
}
