namespace gAMSPro.Helper
{
    public interface IDetailLoggerHelper
    {
        void Logger(string log);
        void ActionLog(string log, string actionName);
        void EndLog(string key);
        string StartLog(string label);
    }
}
