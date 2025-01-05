namespace Core.gAMSPro.CoreModule.Utils
{
    public static class PropertyCopy
    {
        public static void Copy<TSource, TDest>(TSource source, TDest dest)
        {
            var srcProperties = source.GetType().GetProperties();
            var destType = dest.GetType();
            foreach (var srcProperty in srcProperties)
            {
                var destProperty = destType.GetProperty(srcProperty.Name);
                if (destProperty != null)
                {
                    destProperty.SetValue(dest, srcProperty.GetValue(source));
                }
            }
        }
    }
}
