using System.Threading.Tasks;

namespace gAMSPro.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}