using AutoMapper;
using Common.gAMSPro.Models;
namespace Common.gAMSPro
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<TL_MENU, AppMenus.Dto.TL_MENU_ENTITY>();

        }
    }
}