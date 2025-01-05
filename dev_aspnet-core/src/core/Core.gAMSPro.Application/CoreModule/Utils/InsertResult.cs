namespace Core.gAMSPro.Utils
{
    public class InsertResult : CommonResult
    {
        public string Id { get; set; }
        public string Ids { get; set; }

        public int? CarCount { get; set; }
        public int? MotorcycleCount { get; set; }
    }
}
