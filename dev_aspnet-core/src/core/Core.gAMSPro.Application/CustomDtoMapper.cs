using AutoMapper;
using Core.gAMSPro.Consts;
using System;

namespace Core.gAMSPro
{
    public class DateTimeConverterForString : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            try
            {
                return DateTime.ParseExact(source, gAMSProCoreConst.DateTimeFormat,
                                           null);
            }
            catch
            {
                return null;
            }
        }
    }
    public class StringConverterForDateTime : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }
            return source.Value.ToString(gAMSProCoreConst.DateTimeFormat);
        }
    }

    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<string, DateTime?>()
                .ConvertUsing(new DateTimeConverterForString());
            configuration.CreateMap<DateTime?, string>()
                .ConvertUsing(new StringConverterForDateTime());
        }
    }
}