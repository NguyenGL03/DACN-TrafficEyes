using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.Images.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.Images
{
    public interface IImageAppService: IApplicationService
    {
        Task<PagedResultDto<Image_ENTITY>> Image_Search_By_Location(string location );
        Task<PagedResultDto<Image_ENTITY>> Image_Search_By_Location_And_Date_Range(string location, string startDate,string endDate);
        Task<PagedResultDto<Image_ENTITY>> Image_Search_By_Location_And_Date(string location, string date);
        Task<InsertResult> VehicleDetection_Ins( string categoryName, string regionName,int vehicleCount,string imagePath,string detectionAt );
        Task<InsertResult> Image_Update( Image_ENTITY image );
        Task<CommonResult> Image_Delete( string location, string date );


    }
}
