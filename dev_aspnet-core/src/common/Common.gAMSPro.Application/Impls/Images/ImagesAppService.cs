using Abp.Application.Services.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.Images.Dto;
using Common.gAMSPro.Intfs.Images;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;

namespace Common.gAMSPro.Impls.Images
{
    public class ImageAppService : gAMSProCoreAppServiceBase, IImageAppService
    {

        public async Task<InsertResult> VehicleDetection_Ins(string categoryName, string regionName, int vehicleCount, string imagePath, string detectionAt)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.VehicleDetection_Ins, 
                new {
                    CategoryName=categoryName,
                    RegionName=regionName,
                    VehicleCount=vehicleCount,
                    ImagePath=imagePath,
                    DetectionAt=detectionAt
                })).FirstOrDefault();
            return result;
        }

        public async Task<InsertResult> Image_Update(Image_ENTITY image)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.Image_Update, image)).FirstOrDefault();
        }

        public async Task<CommonResult> Image_Delete(string location, string date)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.Image_Delete, new { Location = location, Date = date })).FirstOrDefault();
        }

        public async Task<PagedResultDto<Image_ENTITY>> Image_Search_By_Location(string location)
        {
            var result = await storeProcedureProvider.GetPagingData<Image_ENTITY>(CommonStoreProcedureConsts.Image_Search_By_Location, location);
            return result;
        }

        public async Task<PagedResultDto<Image_ENTITY>> Image_Search_By_Location_And_Date(string location,string date)
        {
            var result = await storeProcedureProvider.GetPagingData<Image_ENTITY>(CommonStoreProcedureConsts.Image_Search_By_Location_And_Date, new{Location=location, Date=date });
            return result;
        }

        public async Task<PagedResultDto<Image_ENTITY>> Image_Search_By_Location_And_Date_Range(string location, string startDate,string endDate)
        {
            var result = await storeProcedureProvider.GetPagingData<Image_ENTITY>(CommonStoreProcedureConsts.Image_Search_By_Location_And_Date_Range, new { Location=location, StartDate=startDate,EndDate=endDate });
            return result;
        }

    }
}